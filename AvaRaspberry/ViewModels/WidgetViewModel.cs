using AvaRaspberry.Interfaces;
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

        public virtual string WidgetTitle
        {
            get => _widgetTitle;
            set => this.RaiseAndSetIfChanged(ref _widgetTitle, value);
        }

        protected static void ProcessEntry(ref List<Tuple<DateTime, Entry>> entries, long newValue,
            SKColor color, float max, DateTime maxEntriesDate, out LineChart cht, long fallBackValue, bool isPositive = true)
        {
            entries = entries.Where(x => x.Item1 > maxEntriesDate).ToList();

            var entry = new Entry()
            {
                Value = isPositive ? newValue : fallBackValue,
                Color = isPositive ? color : App.Red
            };

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

        protected static void ProcessEntry(ref List<Tuple<DateTime, Entry>> entries, long newValue,
            SKColor color, DateTime maxEntriesDate, out List<Tuple<DateTime, Entry>> tickedEntries, long fallBackValue, bool isPositive = true)
        {
            entries = entries.Where(x => x.Item1 > maxEntriesDate).ToList();

            var entry = new Entry()
            {
                Value = isPositive ? newValue : fallBackValue,
                Color = isPositive ? color : App.Red
            };

            entries.Add(new Tuple<DateTime, Entry>(DateTime.Now, entry));

            tickedEntries = entries;
        }

        public static bool ProcessPerMinute(ref List<Tuple<DateTime, Entry>> entries, out List<Tuple<DateTime, Entry>> tickedEntries)
        {

            if (entries.Count < 50)
            {
                tickedEntries = entries;
                return false;
            }
            else
            {
                var minDate = entries.Min(x => x.Item1);
                var maxDate = entries.Max(x => x.Item1);

                var currentMinute = minDate.Minute;
                List<Tuple<DateTime, Entry>> tickedTuple = new();
                tickedEntries = new List<Tuple<DateTime, Entry>>();

                foreach (var tuple in entries)
                {
                    if (currentMinute != tuple.Item1.Minute)
                    {
                        currentMinute = tuple.Item1.Minute;

                        if (tickedTuple.Count > 0)
                        {
                            var entry = new Entry();

                            var last = tickedTuple.Last();

                            if (tickedTuple.Any(x => x.Item2.Color == App.Red))
                            {
                                entry = new Entry()
                                {
                                    Value = tickedTuple.Max(x => x.Item2.Value),
                                    Color = App.Red
                                };
                            }
                            else
                            {
                                entry = new Entry()
                                {
                                    Value = tickedTuple.Average(x => x.Item2.Value),
                                    Color = last.Item2.Color
                                };
                            }

                            var date = new DateTime(last.Item1.Year, last.Item1.Month, last.Item1.Day, last.Item1.Hour, last.Item1.Minute, 0);

                            tickedEntries.Add(new Tuple<DateTime, Entry>(date, entry));
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
                    return false;
                }
                return true;
            }
        }

        public static bool ProcessPerHalfHour(ref List<Tuple<DateTime, Entry>> entries, out List<Tuple<DateTime, Entry>> tickedEntries)
        {

            if (entries.Count < 50)
            {
                tickedEntries = entries;
                return false;
            }
            else
            {
                var minDate = entries.Min(x => x.Item1);
                var maxDate = entries.Max(x => x.Item1);

                var maxTimePerTick = minDate.AddMinutes(30);

                List<Tuple<DateTime, Entry>> tuplesForCurrentTimeRange = new List<Tuple<DateTime, Entry>>();
                tickedEntries = new List<Tuple<DateTime, Entry>>();

                foreach (var tuple in entries)
                {
                    if (tuple.Item1 > maxTimePerTick)
                    {
                        maxTimePerTick = tuple.Item1.AddMinutes(30);

                        if (tuplesForCurrentTimeRange.Count > 0)
                        {

                            var last = tuplesForCurrentTimeRange.Last();
                            var entry = new Entry();

                            if (tuplesForCurrentTimeRange.Any(x => x.Item2.Color == App.Red))
                            {
                                entry = new Entry()
                                {
                                    Value = tuplesForCurrentTimeRange.Max(x => x.Item2.Value),
                                    Color = App.Red
                                };
                            }
                            else
                            {
                                entry = new Entry()
                                {
                                    Value = tuplesForCurrentTimeRange.Average(x => x.Item2.Value),
                                    Color = last.Item2.Color
                                };
                            }


                            var date = new DateTime(last.Item1.Year, last.Item1.Month, last.Item1.Day, last.Item1.Hour, 0, 0);

                            tickedEntries.Add(new Tuple<DateTime, Entry>(date, entry));
                            tuplesForCurrentTimeRange.Clear();
                        }
                    }
                    else
                    {
                        tuplesForCurrentTimeRange.Add(tuple);
                    }
                }

                if (tickedEntries.Count() < 8)
                {
                    tickedEntries = entries;
                    return false;
                }

                return true;
            }
        }
    }
}