using AvaRaspberry.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Microcharts;
using SkiaSharp;

namespace AvaRaspberry.ViewModels
{
    public abstract class WidgetViewModel : ViewModelBase, IWidgetViewModel
    {
        public abstract string WidgetTitle { get; set; }

        protected static void ProcessEntry(List<Entry> entries, long newValue, SKColor color, float max,
            out LineChart cht)
        {
            if (entries.Count == 50)
                entries.RemoveAt(0);


            entries.Add(new Entry()
            {
                Value = newValue,
                // Color = SKColor.Parse("#266489"),
                Color = color
            });

            cht = new LineChart()
            {
                Entries = entries.ToArray(),
                BackgroundColor = SKColor.Parse("#00FFFFFF"),
                PointSize = 0,
                Margin = 0,
                MaxValue = max,
                MinValue = 0
            };
        }
    }
}