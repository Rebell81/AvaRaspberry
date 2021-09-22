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
    public class TorrentViewModel : WidgetViewModel
    {

        public List<Entry> Entries = new List<Entry>();

        private readonly ITorrentComunicator _communicator;
        private readonly Task _updateTask;
        private TorrentClientStatistic? _torrentClientStatistic;
        private string _widgetTitle;
        private LineChart _charts;

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

        public LineChart Chart
        {
            get => _charts;
            protected set => this.RaiseAndSetIfChanged(ref _charts, value);
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

                    if (Entries.Count == 50)
                        Entries.RemoveAt(0);

                    Entries.Add(new Entry()
                    {
                        Value = TorrentClientStatistic.TotalTx,
                        //Label = TorrentClientStatistic.TotalTx.ToString(),
                        //ValueLabel = TorrentClientStatistic.TotalTx.ToString(),
                        Color = SKColor.Parse("#266489")
                    });


                    Chart = new LineChart()
                    {
                        Entries = Entries.ToArray(),
                        BackgroundColor = SKColor.Parse("#00FFFFFF")
                    };
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
    }
}