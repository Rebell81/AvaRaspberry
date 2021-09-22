using Avalonia.Data.Converters;
using System;
using System.Globalization;
using AvaRaspberry.Serivices;

namespace AvaRaspberry.Converters
{
    public class BytesToUserFriendlyText : IValueConverter
    {

        public bool IsSpeed { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Utils.GetSizeString(System.Convert.ToInt64(value), true, IsSpeed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
