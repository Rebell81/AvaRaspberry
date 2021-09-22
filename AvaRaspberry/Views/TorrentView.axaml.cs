using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaRaspberry.Views
{
    public class TorrentView : UserControl
    {
        public TorrentView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}