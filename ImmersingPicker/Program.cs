using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using ClassIsland.Shared.IPC;

namespace ImmersingPicker;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        ClassIslandTest();

        var appBuilder = BuildAvaloniaApp();
        
        appBuilder.StartWithClassicDesktopLifetime(args);
        
        // 应用程序退出时的处理
        if (appBuilder.Instance is App app)
        {
            app.Shutdown();
        }
    }

    public static void ClassIslandTest()
    {
        var client = new IpcClient();
        client.Connect();
        Console.WriteLine("Successfully connect to ClassIsland");
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

}