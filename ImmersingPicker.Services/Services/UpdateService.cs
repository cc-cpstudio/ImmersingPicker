using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ImmersingPicker.Core.Models;
using ImmersingPicker.Services.Helper;
using Serilog;

namespace ImmersingPicker.Services.Services;

/// <summary>
/// GitHub 更新服务 - 负责从 GitHub Releases 获取版本信息
/// </summary>
public class UpdateService
{
    private static readonly UpdateService _instance = new();
    public static UpdateService Instance => _instance;

    private static readonly ILogger _logger = Log.ForContext<UpdateService>();
    private readonly HttpClient _httpClient;

    // GitHub API 端点
    private const string GitHubApiUrl = "https://api.github.com/repos/ImmersingEducation/ImmersingPicker/releases";
    private const string LatestReleaseUrl = GitHubApiUrl + "/latest";
    private const string AllReleasesUrl = GitHubApiUrl;

    private UpdateService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("ImmersingPicker-UpdateChecker");
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    /// <summary>
    /// 检查是否有可用更新
    /// </summary>
    /// <param name="allowPrerelease">是否允许预发布版本</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>检查结果和更新信息</returns>
    public async Task<(UpdateCheckResult Result, UpdateInfo? UpdateInfo)> CheckForUpdatesAsync(
        bool allowPrerelease = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.Information("开始检查更新...");

            UpdateInfo? updateInfo;

            if (allowPrerelease)
            {
                // 获取所有发布版本,找到最新的
                updateInfo = await GetLatestReleaseIncludingPrereleasesAsync(cancellationToken);
            }
            else
            {
                // 仅获取最新稳定版本
                updateInfo = await GetLatestStableReleaseAsync(cancellationToken);
            }

            if (updateInfo == null)
            {
                _logger.Warning("无法获取发布信息");
                return (UpdateCheckResult.CheckFailed, null);
            }

            // 检查用户是否跳过了此版本
            if (updateInfo.Version == AppSettings.Instance.SkippedUpdateVersion)
            {
                _logger.Information("用户已跳过此版本: {Version}", updateInfo.Version);
                return (UpdateCheckResult.Cancelled, null);
            }

            // 比较版本
            var currentVersion = VersionHelper.GetCurrentVersion();
            if (currentVersion == null)
            {
                _logger.Warning("无法获取当前版本");
                return (UpdateCheckResult.CheckFailed, null);
            }

            var comparison = VersionHelper.CompareVersions(currentVersion, updateInfo.Version);
            if (!comparison.HasValue)
            {
                _logger.Warning("版本比较失败");
                return (UpdateCheckResult.CheckFailed, null);
            }

            if (comparison.Value < 0)
            {
                _logger.Information("发现新版本: {Version}", updateInfo.Version);
                return (UpdateCheckResult.UpdateAvailable, updateInfo);
            }

            _logger.Information("当前已是最新版本");
            return (UpdateCheckResult.NoUpdate, null);
        }
        catch (TaskCanceledException ex)
        {
            _logger.Warning(ex, "检查更新超时或已取消");
            return (UpdateCheckResult.Cancelled, null);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "检查更新时发生错误");
            return (UpdateCheckResult.CheckFailed, null);
        }
    }

    /// <summary>
    /// 获取最新稳定版本
    /// </summary>
    private async Task<UpdateInfo?> GetLatestStableReleaseAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<GitHubReleaseResponse>(
                LatestReleaseUrl, 
                GetJsonSerializerOptions(),
                cancellationToken);

            if (response == null)
            {
                _logger.Warning("GitHub API 返回空响应");
                return null;
            }

            return ParseGitHubRelease(response);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "获取最新稳定版本失败");
            return null;
        }
    }

    /// <summary>
    /// 获取最新版本 (包括预发布版本)
    /// </summary>
    private async Task<UpdateInfo?> GetLatestReleaseIncludingPrereleasesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<GitHubReleaseResponse>>(
                AllReleasesUrl,
                GetJsonSerializerOptions(),
                cancellationToken);

            if (response == null || response.Count == 0)
            {
                _logger.Warning("GitHub API 返回空响应");
                return null;
            }

            // 找到最新版本 (包括 prerelease)
            var latestRelease = response.First();
            return ParseGitHubRelease(latestRelease);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "获取最新版本失败");
            return null;
        }
    }

    /// <summary>
    /// 解析 GitHub Release 响应
    /// </summary>
    private UpdateInfo ParseGitHubRelease(GitHubReleaseResponse response)
    {
        var updateInfo = new UpdateInfo
        {
            Version = CleanTagName(response.TagName),
            ReleaseName = response.Name ?? response.TagName,
            ReleaseDate = response.PublishedAt,
            IsPrerelease = response.Prerelease,
            ReleaseNotes = response.Body ?? string.Empty,
            IsMandatory = false // 可以从 release 标签中解析,如 [!mandatory]
        };

        // 查找合适的下载资产 (优先查找 Windows 安装包或便携版)
        var asset = response.Assets
            .FirstOrDefault(a => a.Name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ||
                                 a.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase));

        if (asset != null)
        {
            updateInfo.DownloadUrl = asset.BrowserDownloadUrl;
            updateInfo.FileSize = asset.Size;
            updateInfo.AssetName = asset.Name;
        }
        else
        {
            _logger.Warning("未找到合适的下载资产");
        }

        _logger.Debug("解析发布信息: {Version}, 预发布: {IsPrerelease}, 资产: {AssetName}",
            updateInfo.Version, updateInfo.IsPrerelease, updateInfo.AssetName);

        return updateInfo;
    }

    /// <summary>
    /// 清理标签前缀 (如 "v1.2.3" -> "1.2.3")
    /// </summary>
    private static string CleanTagName(string tagName)
    {
        if (string.IsNullOrEmpty(tagName))
            return string.Empty;

        return tagName.StartsWith("v", StringComparison.OrdinalIgnoreCase) 
            ? tagName[1..] 
            : tagName;
    }

    /// <summary>
    /// 获取 JSON 序列化配置
    /// </summary>
    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    /// <summary>
    /// GitHub Release API 响应模型
    /// </summary>
    private class GitHubReleaseResponse
    {
        public string Url { get; set; } = string.Empty;
        public string TagName { get; set; } = string.Empty;
        public string? Name { get; set; }
        public bool Prerelease { get; set; }
        public DateTime PublishedAt { get; set; }
        public string? Body { get; set; }
        public List<GitHubAsset> Assets { get; set; } = new();
    }

    /// <summary>
    /// GitHub 资产模型
    /// </summary>
    private class GitHubAsset
    {
        public string Name { get; set; } = string.Empty;
        public string BrowserDownloadUrl { get; set; } = string.Empty;
        public long Size { get; set; }
    }
}
