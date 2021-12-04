using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AvaRaspberry.Views
{
    public class GridWidgetView : UserControl
    {
        public GridWidgetView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async void Click(object obj, RoutedEventArgs obj2)
        {
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow.Hide();

                await Task.Delay(5000);
                desktop.MainWindow.Show();


            }
        }
    }

  
}