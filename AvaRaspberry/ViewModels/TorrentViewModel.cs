using System;
using System.Linq;
using System.Threading.Tasks;
using AvaRaspberry.Interfaces;
using SkiaSharp;

namespace AvaRaspberry.ViewModels
{
    public sealed class TorrentViewModel : NetworkChartsViewModel
    {
        private readonly Task _updateTask;

        public TorrentViewModel(INetworkCommunicator communicator, string title, int seconds)
            : base(communicator,seconds)
        {
            _updateTask = Task.Run(Process);
            WidgetTitle = title;
        }


        // public override string WidgetTitle
        // {
        //     get => _widgetTitle;
        //     set => this.RaiseAndSetIfChanged(ref _widgetTitle, value);
        // }

        protected override async Task Process()
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