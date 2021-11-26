using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Microcharts;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Models.Torrent;
using AvaRaspberry.Serivices;
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
        protected NetworkStatisticResponce _torrentClientStatistic;
        private protected readonly INetworkCommunicator Communicator;
        protected int _seconds;
        private long _maxTx;
        private DateTime _start = DateTime.Now;
        private bool log = false;
        private bool _isTitleOverrided;
        public NetworkChartsViewModel()
        {
        }

        public NetworkChartsViewModel(INetworkCommunicator communicator, int seconds, long maxTx, long maxLine, long mediumLine, bool log = false, bool isTitleOverrided = false)
        {
            this.log = log;
            Communicator = communicator;
            _isTitleOverrided = isTitleOverrided;
            _seconds = seconds;
            _maxTx = maxTx;

            FillWithLine(chartLineEntryMax, maxLine, App.Red);
            FillWithLine(chartLineEntryMedium, mediumLine, App.Purple);

            MakeChart(chartLineEntryMax, chartLineEntryMedium);

            _updateTask = Task.Run(GetStats);
            _processTask = Task.Run(Process);
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


        public NetworkStatisticResponce NetworkStatistic
        {
            get => _torrentClientStatistic ?? new NetworkStatisticResponce();
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
                    //var sw = new Stopwatch();
                    //var s2 = new Stopwatch();
                    //s2.Start();
                    //sw.Start();
                    var perMin = false;

                    //ConsoleLog(sw, WidgetTitle, "1");

                    ProcessEntry(ref _entriesTx, NetworkStatistic.TotalTx,
                        App.Green, DateTime.Now.AddSeconds(-_seconds), out _tickedEntriesTx, _maxTx, NetworkStatistic.Result);

                    //ConsoleLog(sw, WidgetTitle, "2");

                    ProcessEntry(ref _entriesRx, NetworkStatistic.TotalRx,
                        App.Blue, DateTime.Now.AddSeconds(-_seconds), out _tickedEntriesRx, _maxTx, NetworkStatistic.Result);

                    //ConsoleLog(sw, WidgetTitle, "3");


                    if (_tickedEntriesRx.Count > 50)
                    {
                        perMin = ProcessPerMinute(ref _entriesRx, out _tickedEntriesRx);
                    }
                    //ConsoleLog(sw, WidgetTitle, "4");


                    if (perMin)
                    {
                        ProcessPerMinute(ref _entriesTx, out _tickedEntriesTx);
                    }
                    //ConsoleLog(sw, WidgetTitle, "5");


                    var perHour = false;

                    if (_tickedEntriesTx.Count > 50 && perMin)
                    {
                        perHour = ProcessPerHalfHour(ref _tickedEntriesTx, out var tickedEntriesTx);
                        _tickedEntriesTx = tickedEntriesTx;
                    }
                    //ConsoleLog(sw, WidgetTitle, "6");


                    if (perHour)
                    {
                        ProcessPerHalfHour(ref _tickedEntriesRx, out var tickedEntriesRx);
                        _tickedEntriesRx = tickedEntriesRx;
                    }
                    //ConsoleLog(sw, WidgetTitle, "7");



                    var span = DateTime.Now - _start;

                    if (!_isTitleOverrided)
                        WidgetTitle = $"{span.Humanize()} | H:{perHour} M:{perMin}| Rx: {_tickedEntriesRx.Count()} | Tx: {_tickedEntriesTx.Count()}";


                    //ConsoleLog(sw, WidgetTitle, "8");
                    ChartTx = new LineChart()
                    {
                        Entries = _tickedEntriesTx.Select(x => x.Item2).ToArray(),
                        BackgroundColor = NetworkStatistic.Result ? App.Tranparent : App.Red,
                        PointSize = 0,
                        Margin = 0,
                        MaxValue = _maxTx,
                        //MaxValue = _tickedEntriesTx.Max(x=>x.Item2.Value),
                        MinValue = 0
                    };
                    //ConsoleLog(sw, WidgetTitle, "9");

                    ChartRx = new LineChart()
                    {
                        Entries = _tickedEntriesRx.Select(x => x.Item2).ToArray(),
                        BackgroundColor = NetworkStatistic.Result ? App.Tranparent : App.Red,
                        PointSize = 0,
                        Margin = 0,
                        MaxValue = _maxTx,
                        //MaxValue = _tickedEntriesRx.Max(x => x.Item2.Value),
                        MinValue = 0
                    };

                    //s2.Stop();

                    //ConsoleLog(sw, WidgetTitle, "10");
                    if (log)
                    {

                        Console.WriteLine($" {Thread.CurrentThread.ManagedThreadId} total | {s2.Elapsed.Humanize()} | {s2.Elapsed.TotalMilliseconds}");
                        Debug.WriteLine($" {Thread.CurrentThread.ManagedThreadId} total | {s2.Elapsed.Humanize()} | {s2.Elapsed.TotalMilliseconds}");

                        Console.WriteLine($"---------------------------------------------------------------------------------------------------------------------------------------------------------");
                        Debug.WriteLine($"---------------------------------------------------------------------------------------------------------------------------------------------------------");

                    }
                }
                catch (Exception ex)
                {
                    LoggerService.Instance.Log(ex);
                }
                finally
                {
                    await Task.Delay(App.GlobalDelay);
                }
            }

        }

        private void ConsoleLog(Stopwatch time, string who, string step)
        {
            if (log)
            {
                Console.WriteLine($" {Thread.CurrentThread.ManagedThreadId} {step} | {time.Elapsed.Humanize()} | {time.Elapsed.TotalMilliseconds}");
                Debug.WriteLine($" {Thread.CurrentThread.ManagedThreadId} {step} | {time.Elapsed.Humanize()} | {time.Elapsed.TotalMilliseconds}");
                time.Stop();
                time.Reset();
                time.Start();
            }

        }
    }
}
