using Avalonia.Data.Converters;
using System;
using System.Globalization;
using AvaRaspberry.Serivices;
using AvaRaspberry.Extenstion;
using Humanizer;

namespace AvaRaspberry.Converters
{
    public class BytesToUserFriendlyText : IValueConverter
    {

        public bool IsSpeed { get; set; }

        public object Convert(object value)
        {
            var addition = IsSpeed ? "/s" : string.Empty;
            return ((long)(value)).Bytes().ToString("#.##") + addition;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
