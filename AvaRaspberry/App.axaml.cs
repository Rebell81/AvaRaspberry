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

        public static ServiceProvider? ServiceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            ServiceProvider = serviceCollection.BuildServiceProvider();

        }


        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
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