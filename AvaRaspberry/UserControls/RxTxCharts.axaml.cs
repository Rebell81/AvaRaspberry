using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaRaspberry.UserControls
{
    public class RxTxCharts : UserControl
    {
        public RxTxCharts()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}