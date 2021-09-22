using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaRaspberry.ViewModels;
using AvaRaspberry.Views;
using Microsoft.Extensions.DependencyInjection;
namespace AvaRaspberry
{
    public class App : Application
    {

        public static int GlobalDelay = 1500;

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