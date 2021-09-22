using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Microcharts;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Models.Torrent;
using ReactiveUI;
using SkiaSharp;

namespace AvaRaspberry.ViewModels
{
    public class TorrentViewModel : WidgetViewModel, INotifyPropertyChanged
    {
        private List<Entry> _entriesTx = new List<Entry>();
        private List<Entry> _entriesRx = new List<Entry>();

        private readonly ITorrentComunicator _communicator;
        private readonly Task _updateTask;
        private TorrentClientStatistic? _torrentClientStatistic;
        private string _widgetTitle;
        private LineChart _chartTx, _chartRx;

        public TorrentViewModel(ITorrentComunicator communicator, string title)
        {
            _communicator = communicator;
            _updateTask = Task.Run(StartUpdate);
            _widgetTitle = title;

            //Chart = new LineChart() { Entries = this.Entries };
        }

        public TorrentClientStatistic TorrentClientStatistic
        {
            get => _torrentClientStatistic ?? new TorrentClientStatistic();
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

        public override string WidgetTitle
        {
            get => _widgetTitle;
            set => this.RaiseAndSetIfChanged(ref _widgetTitle, value);
        }

        private async Task StartUpdate()
        {
            while (true)
            {
                try
                {
                    TorrentClientStatistic = await _communicator.GetStatisticData();

                    float max = 0;
                    if (ChartTx?.Entries != null)
                    {
                        var array = ChartTx.Entries.Concat(ChartRx.Entries);
                        max = array.Max(x => x.Value);
                    }
                   


                    ProcessEntry(_entriesTx, TorrentClientStatistic.TotalTx,
                        SKColor.Parse("#66BF11"), max, out var chartTx);

                    ProcessEntry(_entriesRx, TorrentClientStatistic.TotalRx,
                        SKColor.Parse("#385AE3"), max, out var chartRx);

                    ChartTx = chartTx;
                    ChartRx = chartRx;
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

        //public new event PropertyChangedEventHandler? PropertyChanged;


        // protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        // {
        //     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        // }
    }
}