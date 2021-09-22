using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaRaspberry.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Console.WriteLine("MainWindow S");

            InitializeComponent();
            Console.WriteLine("MainWindow m");

#if DEBUG
            Avalonia.DevToolsExtensions.AttachDevTools(this);
            WindowState = WindowState.Normal;
#else
            WindowState = WindowState.FullScreen;
            Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.None);
            SystemDecorations = SystemDecorations.None;
#endif
            Console.WriteLine("MainWindow e");

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}