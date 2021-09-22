using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaRaspberry.Views
{
    public class EmptyWidgetView : UserControl
    {
        public EmptyWidgetView()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static readonly AvaloniaProperty TextProperty = AvaloniaProperty.Register<EmptyWidgetView, string>(nameof(Text), string.Empty, false, Avalonia.Data.BindingMode.TwoWay);


        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
    }
}