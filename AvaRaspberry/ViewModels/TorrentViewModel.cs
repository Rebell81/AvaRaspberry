using System;
using System.Linq;
using System.Threading.Tasks;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Serivices;
using SkiaSharp;

namespace AvaRaspberry.ViewModels
{
    public sealed class TorrentViewModel : NetworkChartsViewModel
    {
        private readonly Task _updateTask;

        public TorrentViewModel(INetworkCommunicator communicator, string title, int seconds, long maxLine, long mediumLine)
            : base(communicator, seconds, App.TorrentMaxTx, maxLine, mediumLine)
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
                    //if (ChartTx?.Entries != null)
                    //{
                    //    var array = ChartTx.Entries.Concat(ChartRx.Entries);
                    //    if (array.Count() > 0)
                    //        max = array.Max(x => x.Value);
                    //}

                    ProcessEntry(ref _entriesTx, NetworkStatistic.TotalTx,
                        SKColor.Parse("#66BF11"), App.TorrentMaxTx, DateTime.Now.AddSeconds(-_seconds), out var chartTx);

                    ProcessEntry(ref _entriesRx, NetworkStatistic.TotalRx,
                        SKColor.Parse("#385AE3"), App.TorrentMaxTx, DateTime.Now.AddSeconds(-_seconds), out var chartRx);

                    ChartTx = chartTx;
                    ChartRx = chartRx;
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(ex);
                }
                finally
                {
                    await Task.Delay(App.GlobalDelay);
                }
            }
        }
    }
}