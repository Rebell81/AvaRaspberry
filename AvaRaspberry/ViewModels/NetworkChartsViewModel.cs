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

        private LineChart _chartTx, _chartRx;
        private readonly Task _updateTask;

        private readonly Task _processTask;
        protected NetworkStatistic _torrentClientStatistic;
        private protected readonly INetworkCommunicator Communicator;
        protected int _seconds;
        private long _maxTx;


        public NetworkChartsViewModel()
        {
        }

        public NetworkChartsViewModel(INetworkCommunicator communicator, int seconds, long maxTx)
        {
            Communicator = communicator;
            _updateTask = Task.Run(GetStats);
            _processTask = Task.Run(Process);
            _seconds = seconds;
            _maxTx = maxTx;
            WidgetTitle = $"{TimeSpan.FromSeconds(seconds).TotalHours} h.";

            //Chart = new LineChart() { Entries = this.Entries };
        }

        public NetworkStatistic NetworkStatistic
        {
            get => _torrentClientStatistic ?? new NetworkStatistic();
            protected set => this.RaiseAndSetIfChanged(ref _torrentClientStatistic, value);
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


                    ProcessEntry(ref _entriesTx, NetworkStatistic.TotalTx,
                        SKColor.Parse("#66BF11"), DateTime.Now.AddSeconds(-_seconds));

                    ProcessEntry(ref _entriesRx, NetworkStatistic.TotalRx,
                        SKColor.Parse("#385AE3"), DateTime.Now.AddSeconds(-_seconds));

                    ProcessPerMinute(ref _entriesTx, out _tickedEntriesTx);
                    ProcessPerMinute(ref _entriesRx, out _tickedEntriesRx);



                    UpdateWidget(_entriesTx.Count > 0 ? _entriesTx : _entriesRx);


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

        private void UpdateWidget(IEnumerable<Tuple<DateTime, Entry>> list)
        {
            if (list.Count() > 0)
            {
                var startDate = list.Min(x => x.Item1);
                var endDate = list.Max(x => x.Item1);
                var span = endDate - startDate;
                WidgetTitle = $"{span.Humanize()}";
            }
        }
    }
}