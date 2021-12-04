using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Humanizer;
using LiveChartsCore.Kernel;

namespace AvaRaspberry.Converters
{
    public class BytesToUserFriendlyText : IValueConverter
    {

        public bool IsSpeed { get; set; }

        public object Convert(object value)
        {
            var addition = IsSpeed ? "/s" : string.Empty;

            var bools = long.TryParse(value.ToString(), out var longVal);

            if (bools)
                return longVal.Bytes().ToString("#.##") + addition;
            return value;
        }

        //public object Convert(TypedChartPoint value)
        //{
        //    var addition = IsSpeed ? "/s" : string.Empty;

        //    var longVal = long.Parse(value.ToString());

        //    return longVal.Bytes().ToString("#.##") + addition;
        //}


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
