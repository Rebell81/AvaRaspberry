﻿using System;
using Avalonia;
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
        private static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .BuildConfiguration()
                .UseReactiveUI();
    }
}
