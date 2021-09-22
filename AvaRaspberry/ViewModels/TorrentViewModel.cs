using System.Threading.Tasks;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Models.Torrent;
using ReactiveUI;

namespace AvaRaspberry.ViewModels
{
    public class TorrentViewModel : WidgetViewModel
    {
        private readonly ITorrentComunicator _communicator;
        private readonly Task _updateTask;
        private TorrentClientStatistic? _torrentClientStatistic;
        private string _widgetTitle;


        public TorrentViewModel(ITorrentComunicator communicator, string title)
        {
            _communicator = communicator;
            _updateTask = Task.Run(StartUpdate);
            _widgetTitle = title;
        }

        public TorrentClientStatistic TorrentClientStatistic
        {
            get => _torrentClientStatistic ?? new TorrentClientStatistic();
            protected set => this.RaiseAndSetIfChanged(ref _torrentClientStatistic, value);
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