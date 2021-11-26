using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaRaspberry.Serivices;
using AvaRaspberry.ViewModels;
using AvaRaspberry.Views;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;

namespace AvaRaspberry
{
    public class App : Application
    {

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LoggerService.Instance.Log(e.Exception);
            };
            
        }

        
        
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LoggerService.Instance.Log(e.ExceptionObject);
        }

        public static int GlobalDelay = 1500;
        public static SKColor Green = SKColor.Parse("#66BF11");
        public static SKColor Blue = SKColor.Parse("#385AE3");
        public static SKColor Red = SKColor.Parse("#b30000");
        public static SKColor Purple = SKColor.Parse("#7A1189");

        public static long TorrentMaxTx = 31457280; //30 mibibits
        public static long SynologyMaxTx = 102674512; //1 gibibit


        public static long TorrentMaxTxLine = 20971520; //20 mibibits
        public static long TorrentMediumTxLine = 10485760; //10 mibibits

        public static long SynologyMaxTxLine = 53687091; //0.5gibibit
        public static long SynologyMediumTxLine = 20971520; //0.2 gibibit

        public static SKColor Tranparent = SKColor.Parse("#00FFFFFF");

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