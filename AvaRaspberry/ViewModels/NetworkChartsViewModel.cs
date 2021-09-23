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
            while (true)
            {
                try
                {
                    //if (ChartTx?.Entries != null)
                    //{
                    //    var array = ChartTx.Entries.Concat(ChartRx.Entries);
                    //    if (array.Count() > 0)
                    //        max = array.Max(x => x.Value);
                    //}


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
                        perHour = ProcessPerHalfHour(ref _tickedEntriesTx, out _tickedEntriesTx);
                    }


                    if (perHour)
                    {
                        ProcessPerHalfHour(ref _tickedEntriesRx, out _tickedEntriesRx);
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
                        MinValue = 0
                    };

                    ChartRx = new LineChart()
                    {
                        Entries = _tickedEntriesRx.Select(x => x.Item2).ToArray(),
                        BackgroundColor = SKColor.Parse("#00FFFFFF"),
                        PointSize = 0,
                        Margin = 0,
                        MaxValue = _maxTx,
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