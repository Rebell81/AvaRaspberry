using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Microcharts;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Models.Torrent;
using ReactiveUI;
using SkiaSharp;

namespace AvaRaspberry.ViewModels
{
    public class NetworkChartsViewModel : WidgetViewModel
    {
        protected List<Tuple<DateTime, Entry>> _entriesTx = new List<Tuple<DateTime, Entry>>();
        protected List<Tuple<DateTime, Entry>> _entriesRx = new List<Tuple<DateTime, Entry>>();
        private LineChart _chartTx, _chartRx;
        private readonly Task _updateTask;
        private readonly Task _processTask;
        protected NetworkStatistic _torrentClientStatistic;
        private protected readonly INetworkCommunicator Communicator;
        protected int _seconds;

        public NetworkChartsViewModel()
        {
        }

        public NetworkChartsViewModel(INetworkCommunicator communicator, int seconds)
        {
            Communicator = communicator;
            _updateTask = Task.Run(GetStats);
            _processTask = Task.Run(Process);
            _seconds = seconds;
            
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

        public override string WidgetTitle { get; set; }

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
                    float max = 0;
                    if (ChartTx?.Entries != null)
                    {
                        var array = ChartTx.Entries.Concat(ChartRx.Entries);
                        max = array.Max(x => x.Value);
                    }


                    ProcessEntry(ref _entriesTx, NetworkStatistic.TotalTx,
                        SKColor.Parse("#66BF11"), max, DateTime.Now.AddSeconds(-_seconds), out var chartTx);

                    ProcessEntry(ref _entriesRx, NetworkStatistic.TotalRx,
                        SKColor.Parse("#385AE3"), max, DateTime.Now.AddSeconds(-_seconds), out var chartRx);

                    ChartTx = chartTx;
                    ChartRx = chartRx;
                }
                catch
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