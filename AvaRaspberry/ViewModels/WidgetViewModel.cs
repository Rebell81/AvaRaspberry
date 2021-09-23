﻿using AvaRaspberry.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Microcharts;
using SkiaSharp;
using ReactiveUI;

namespace AvaRaspberry.ViewModels
{
    public abstract class WidgetViewModel : ViewModelBase, IWidgetViewModel
    {

        private string _widgetTitle;

        public string WidgetTitle
        {
            get => _widgetTitle;
            set => this.RaiseAndSetIfChanged(ref _widgetTitle, value);
        }

        protected static void ProcessEntry(ref List<Tuple<DateTime, Entry>> entries, long newValue,
            SKColor color, float max, DateTime maxEntriesDate, out LineChart cht)
        {
            entries = entries.Where(x => x.Item1 > maxEntriesDate).ToList();

            var entry = new Entry()
            {
                Value = newValue,
                Color = color
            };

            if (newValue > 0)
                entries.Add(new Tuple<DateTime, Entry>(DateTime.Now, entry));

            cht = new LineChart()
            {
                Entries = entries.Select(x => x.Item2).ToArray(),
                BackgroundColor = SKColor.Parse("#00FFFFFF"),
                PointSize = 0,
                Margin = 0,
                MaxValue = max,
                MinValue = 0
            };
        }

        public static void ProcessPerMinute(ref List<Tuple<DateTime, Entry>> entries, out List<Tuple<DateTime, Entry>> tickedEntries)
        {

            if (entries.Count < 50)
            {
                tickedEntries = entries;
                return;
            }
            else
            {
                var minDate = entries.Min(x => x.Item1);
                var maxDate = entries.Max(x => x.Item1);

                var currentMinute = minDate.Minute;
                List<Tuple<DateTime, Entry>> tickedTuple = new List<Tuple<DateTime, Entry>>();
                tickedEntries = new List<Tuple<DateTime, Entry>>();

                foreach (var tuple in entries)
                {
                    if (currentMinute != tuple.Item1.Minute)
                    {
                        currentMinute = tuple.Item1.Minute;

                        if(tickedTuple.Count>0)
                        {
                            var last = tickedTuple.Last();
                            var avarage = tickedTuple.Average(x => x.Item2.Value);
                            last.Item2.Value = avarage;
                            tickedEntries.Add(new Tuple<DateTime, Entry>(last.Item1, last.Item2));
                            tickedTuple.Clear();
                        }
                    }
                    else
                    {
                        tickedTuple.Add(tuple);
                    }
                }

                if (tickedEntries.Count() < 8)
                {
                    tickedEntries = entries;
                    return;
                }
            }
        }
    }
}