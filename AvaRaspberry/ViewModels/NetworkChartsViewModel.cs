using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Microcharts;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Models.Torrent;
using Humanizer;
using ReactiveUI;
using SkiaSharp;

namespace AvaRaspberry.ViewModels
{
    public class NetworkChartsViewModel : WidgetViewModel
    {
        protected List<Tuple<DateTime, Entry>> _entriesTx = new List<Tuple<DateTime, Entry>>();
        protected List<Tuple<DateTime, Entry>> _entriesRx = new List<Tuple<DateTime, Entry>>();

        private List<Tuple<DateTime, Entry>> _tickedEntriesTx = new List<Tuple<DateTime, Entry>>();
        private List<Tuple<DateTime, Entry>> _tickedEntriesRx = new List<Tuple<DateTime, Entry>>();

        private List<Entry> chartLineEntryMax = new List<Entry>();
        private List<Entry> chartLineEntryMedium = new List<Entry>();


        private LineChart _chartTx, _chartRx, _chartLineMax, _chartLineMedium;
        private readonly Task _updateTask;

        private readonly Task _processTask;
        protected NetworkStatistic _torrentClientStatistic;
        private protected readonly INetworkCommunicator Communicator;
        protected int _seconds;
        private long _maxTx;
        private DateTime _start = DateTime.Now;

        public NetworkChartsViewModel()
        {
        }

        public NetworkChartsViewModel(INetworkCommunicator communicator, int seconds, long maxTx, long maxLine, long mediumLine)
        {
            Communicator = communicator;
            _updateTask = Task.Run(GetStats);
            _processTask = Task.Run(Process);
            _seconds = seconds;
            _maxTx = maxTx;
            WidgetTitle = $"{TimeSpan.FromSeconds(seconds).TotalHours} h.";

            FillWithLine(chartLineEntryMax, maxLine, App.Red);
            FillWithLine(chartLineEntryMedium, mediumLine, App.Purple);

            MakeChart(chartLineEntryMax, chartLineEntryMedium);
        }

        private void MakeChart(List<Entry> entriesMax, List<Entry> entriesMedium)
        {
            ChartLineMedium = new LineChart()
            {
                Entries = entriesMedium.ToArray(),
                BackgroundColor = App.Tranparent,
                PointSize = 0,
                Margin = 0,
                MaxValue = _maxTx,
                MinValue = 0,
                LineSize = 1,
                PointMode = PointMode.None,
                LineAreaAlpha = 0
            };

            ChartLineMax = new LineChart()
            {
                Entries = entriesMax.ToArray(),
                BackgroundColor = App.Tranparent,
                PointSize = 0,
                Margin = 0,
                MaxValue = _maxTx,
                MinValue = 0,
                LineAreaAlpha = 0,

                LineSize = 1,
                PointMode = PointMode.None
            };
        }


        private void FillWithLine(List<Entry> entries, long value, SKColor color)
        {
            var entry = new Entry()
            {
                Value = value,
                Color = color,
            };

            for (int i = 0; i <= 100; i++)
            {
                entries.Add(entry);
            }
        }


        public NetworkStatistic NetworkStatistic
        {
            get => _torrentClientStatistic ?? new NetworkStatistic();
            protected set => this.RaiseAndSetIfChanged(ref _torrentClientStatistic, value);
        }

        public LineChart ChartLineMax
        {
            get => _chartLineMax;
            protected set => this.RaiseAndSetIfChanged(ref _chartLineMax, value);
        }

        public LineChart ChartLineMedium
        {
            get => _chartLineMedium;
            protected set => this.RaiseAndSetIfChanged(ref _chartLineMedium, value);
        }



        public LineChart ChartRx
        {
            get => _chartRx;
            protected set => this.RaiseAndSetIfChanged(ref _chartRx, value);
        }

        public LineChart ChartTx
        {
            get => _chartTx;
            protected set => this.RaiseAndSetIfChanged(ref _chartTx, value);
        }


        private async Task GetStats()
        {
            while (true)
            {
                try
                {
                    NetworkStatistic = await Communicator.GetNetworkStatisticData();
                }
                catch
                {
                    // ignored
                }
                finally
                {
                    await Task.Delay(App.GlobalDelay);
                }
            }
        }


        protected virtual async Task Process()
        {
            //var entr2y = new Entry()
            //{
            //    Value = new Random().Next(100000, 500000),
            //    Color = App.Blue
            //};

            //var entr22y = new Entry()
            //{
            //    Value = new Random().Next(100000, 500000),
            //    Color = App.Green
            //};

            //_entriesTx.Add(new Tuple<DateTime, Entry>(DateTime.Now, entr2y));
            //_entriesRx.Add(new Tuple<DateTime, Entry>(DateTime.Now, entr22y));

            //for (var i = 0; i <= 100000; i++)
            //{
            //    var lst = _entriesTx.Last();
            //    var entry = new Entry()
            //    {
            //        Value = new Random().Next(100000, 500000),
            //        Color = App.Blue
            //    };

            //    _entriesTx.Add(new Tuple<DateTime, Entry>(lst.Item1.AddSeconds(1), entry));

            //    var lst2 = _entriesRx.Last();
            //    var entry2 = new Entry()
            //    {
            //        Value = new Random().Next(100000, 500000),
            //        Color = App.Green
            //    };

            //    _entriesRx.Add(new Tuple<DateTime, Entry>(lst2.Item1.AddSeconds(1), entry2));
            //}


            while (true)
            {
                try
                {

                    var perMin = false;


                    ProcessEntry(ref _entriesTx, NetworkStatistic.TotalTx,
                        SKColor.Parse("#66BF11"), DateTime.Now.AddSeconds(-_seconds), out _tickedEntriesTx);

                    ProcessEntry(ref _entriesRx, NetworkStatistic.TotalRx,
                        SKColor.Parse("#385AE3"), DateTime.Now.AddSeconds(-_seconds), out _tickedEntriesRx);


                    if (_tickedEntriesRx.Count > 50)
                    {
                        perMin = ProcessPerMinute(ref _entriesRx, out _tickedEntriesRx);
                    }


                    if (perMin)
                    {
                        ProcessPerMinute(ref _entriesTx, out _tickedEntriesTx);
                    }


                    var perHour = false;

                    if (_tickedEntriesTx.Count > 50 && perMin)
                    {
                        perHour = ProcessPerHalfHour(ref _tickedEntriesTx, out var tickedEntriesTx);
                        _tickedEntriesTx = tickedEntriesTx;
                    }


                    if (perHour)
                    {
                        ProcessPerHalfHour(ref _tickedEntriesRx, out var tickedEntriesRx);
                        _tickedEntriesRx = tickedEntriesRx;
                    }



                    var span = DateTime.Now - _start;
                    WidgetTitle = $"{span.Humanize()} | H:{perHour} M:{perMin}| Rx: {_tickedEntriesRx.Count()} | Tx: {_tickedEntriesTx.Count()}";

                    ChartTx = new LineChart()
                    {
                        Entries = _tickedEntriesTx.Select(x => x.Item2).ToArray(),
                        BackgroundColor = SKColor.Parse("#00FFFFFF"),
                        PointSize = 0,
                        Margin = 0,
                        MaxValue = _maxTx,
                        //MaxValue = _tickedEntriesTx.Max(x=>x.Item2.Value),
                        MinValue = 0
                    };

                    ChartRx = new LineChart()
                    {
                        Entries = _tickedEntriesRx.Select(x => x.Item2).ToArray(),
                        BackgroundColor = SKColor.Parse("#00FFFFFF"),
                        PointSize = 0,
                        Margin = 0,
                        MaxValue = _maxTx,
                        //MaxValue = _tickedEntriesRx.Max(x => x.Item2.Value),
                        MinValue = 0
                    };


                }
                catch (Exception ex)
                {
                }
                finally
                {
                    await Task.Delay(App.GlobalDelay);
                }
            }

        }
    }
}