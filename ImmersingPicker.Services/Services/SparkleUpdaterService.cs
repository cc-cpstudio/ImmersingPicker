using System;
using NetSparkleUpdater;
using Serilog;

namespace ImmersingPicker.Services.Services;

public class SparkleUpdaterService
{
    private static SparkleUpdaterService _instance = null!;
    public static SparkleUpdaterService Instance => _instance ??= new SparkleUpdaterService();
    
    private static readonly Serilog.ILogger _logger = Log.ForContext<SparkleUpdaterService>();
    
    private SparkleUpdater? _sparkle;
    private bool _initialized = false;
    
    private SparkleUpdaterService()
    {
    }
    
    // 此方法将在主项目中调用
    public void SetSparkleUpdater(SparkleUpdater sparkle)
    {
        _sparkle = sparkle;
        _initialized = true;
        
        _logger.Information("SparkleUpdater 已设置到服务中");
    }
    
    // 访问 SparkleUpdater 实例的属性
    public SparkleUpdater? Sparkle => _sparkle;
    
    public void StartUpdateLoop(bool checkOnStart = true)
    {
        if (!_initialized || _sparkle == null)
        {
            _logger.Warning("SparkleUpdaterService 未初始化，无法启动更新循环");
            return;
        }
        
        _logger.Information("启动更新循环");
        _sparkle.StartLoop(checkOnStart);
    }
    
    public void StopUpdateLoop()
    {
        if (!_initialized || _sparkle == null)
        {
            return;
        }
        
        _logger.Information("停止更新循环");
        _sparkle.StopLoop();
    }
    
    public void CheckForUpdatesAtUserRequest()
    {
        if (!_initialized || _sparkle == null)
        {
            _logger.Warning("SparkleUpdaterService 未初始化，无法检查更新");
            return;
        }
        
        _logger.Information("用户请求检查更新");
        _sparkle.CheckForUpdatesAtUserRequest();
    }
    
    public bool IsInitialized => _initialized;
}
