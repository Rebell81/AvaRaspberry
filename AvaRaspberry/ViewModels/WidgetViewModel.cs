using AvaRaspberry.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Microcharts;
using SkiaSharp;

namespace AvaRaspberry.ViewModels
{
    public abstract class WidgetViewModel : ViewModelBase, IWidgetViewModel
    {
        public abstract string WidgetTitle { get; set; }

        protected static void ProcessEntry(ref List<Tuple<DateTime, Entry>> entries, long newValue,
            SKColor color, float max, DateTime maxEntriesDate, out LineChart cht)
        {
            entries = entries.Where(x => x.Item1 > maxEntriesDate).ToList();

            var entry = new Entry()
            {
                Value = newValue,
                // Color = SKColor.Parse("#266489"),
                Color = color
            };
            
            entries.Add(new Tuple<DateTime, Entry>(DateTime.Now, entry));

            cht = new LineChart()
            {
                Entries = entries.Select(x=>x.Item2).ToArray(),
                BackgroundColor = SKColor.Parse("#00FFFFFF"),
                PointSize = 0,
                Margin = 0,
                MaxValue = max,
                MinValue = 0
            };
        }
    }
}