using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace AvaRaspberry.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
            WindowState = WindowState.Normal;
#else
            WindowState = WindowState.FullScreen;
            Cursor = new Cursor(StandardCursorType.None);
            SystemDecorations = SystemDecorations.None;
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}