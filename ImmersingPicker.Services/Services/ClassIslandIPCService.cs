using System;
using ClassIsland.Shared.Enums;
using ClassIsland.Shared.IPC;
using ClassIsland.Shared.IPC.Abstractions.Services;
using dotnetCampus.Ipc.CompilerServices.GeneratedProxies;
using ImmersingPicker.Core.Exceptions;
using Serilog;

namespace ImmersingPicker.Services.Services;

public class ClassIslandIPCService
{
    public static ClassIslandIPCService Instance = new ClassIslandIPCService();

    private IpcClient? _client;
    public bool Initialized = false;
    private readonly object _lock = new object();

    public async Task InitializeAsync()
    {
        if (Initialized) return;

        lock (_lock)
        {
            if (Initialized) return;
            
            _client = new IpcClient();
        }

        try
        {
            await _client.Connect();
            Initialized = true;
            Log.Information("ClassIsland IPC 连接成功");
        }
        catch (OperationCanceledException ex)
        {
            Log.Warning(ex, "ClassIsland IPC 连接被取消");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "ClassIsland IPC 初始化失败");
            throw new ClassIslandIPCInitializationFailed();
        }
    }

    public void Initialize()
    {
        _ = InitializeAsync();
    }

    public bool OnClass()
    {
        if (!Initialized || _client == null)
        {
            return false;
        }

        try
        {
            if (_client.PeerProxy == null)
            {
                return false;
            }

            var lessonSc = _client.Provider.CreateIpcProxy<IPublicLessonsService>(_client.PeerProxy);
            return lessonSc.CurrentState is TimeState.OnClass or TimeState.PrepareOnClass or TimeState.None
                   && lessonSc.IsLessonConfirmed;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "获取 ClassIsland 课程状态失败");
            return false;
        }
    }
}
