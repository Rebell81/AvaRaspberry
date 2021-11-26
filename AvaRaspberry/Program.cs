using System;
using System.Threading;
using Avalonia;
using Avalonia.Dialogs;
using Avalonia.ReactiveUI;
using AvaRaspberry.Extenstion;

namespace AvaRaspberry
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break. 
        public static void Main(string[] args)
        {
            Console.WriteLine("MainStart");
            
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
            Console.WriteLine("MainEnd");
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
             => AppBuilder.Configure<App>()
                 .UsePlatformDetect()
                 .LogToDebug()
             .With(new X11PlatformOptions
             {
                 EnableMultiTouch = true,
                 UseDBusMenu = true
             })
             .With(new Win32PlatformOptions
             {
                 EnableMultitouch = true,
                 AllowEglInitialization = true
             })
             .UseSkia()
                 .UseReactiveUI()
             .UseManagedSystemDialogs();

        static void SilenceConsole()
        {
            new Thread(() =>
            {
                Console.CursorVisible = false;
                while (true)
                    Console.ReadKey(true);
            })
            {
                IsBackground = true
            }.Start();
        }
    }
}