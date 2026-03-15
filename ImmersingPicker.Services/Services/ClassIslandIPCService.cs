using ClassIsland.Shared.IPC;
using ClassIsland.Shared.IPC.Abstractions.Services;
using dotnetCampus.Ipc.CompilerServices.GeneratedProxies;
using Serilog;

namespace ImmersingPicker.Services.Services;

public class ClassIslandIPCService
{
    public static ClassIslandIPCService Instance = new ClassIslandIPCService();

    public event Action CI_OnClass;
    public event Action CI_OnBreakingTimeOrAfterSchool;

    private bool _initialized = false;

    public async void Initialize()
    {
        if (_initialized) return;

        var client = new IpcClient();

        client.JsonIpcProvider.AddNotifyHandler(IpcRoutedNotifyIds.OnClassNotifyId, () =>
        {
            Log.Information("ClassIsland 通知：开始上课");
            CI_OnClass?.Invoke();
        });
        client.JsonIpcProvider.AddNotifyHandler(IpcRoutedNotifyIds.OnBreakingTimeNotifyId, () =>
        {
            Log.Information("ClassIsland 通知：开始课间休息");
            CI_OnBreakingTimeOrAfterSchool?.Invoke();
        });
        client.JsonIpcProvider.AddNotifyHandler(IpcRoutedNotifyIds.OnAfterSchoolNotifyId, () =>
        {
            Log.Information("ClassIsland 通知：放学");
            CI_OnBreakingTimeOrAfterSchool?.Invoke();
        });

        await client.Connect();
    }
}