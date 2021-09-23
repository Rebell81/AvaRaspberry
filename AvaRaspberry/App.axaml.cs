using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaRaspberry.ViewModels;
using AvaRaspberry.Views;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;

namespace AvaRaspberry
{
    public class App : Application
    {

        public static int GlobalDelay = 1500;
        public static SKColor Green = SKColor.Parse("#66BF11");
        public static SKColor Blue = SKColor.Parse("#385AE3");
        public static long TorrentMaxTx = 31457280; //30 mibibits
        public static long SynologyMaxTx = 102674512; //1 gibibit

        public override void Initialize()
        {
            Console.WriteLine("Initialize Start");
            AvaloniaXamlLoader.Load(this);
            Console.WriteLine("Initialize End");

        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}