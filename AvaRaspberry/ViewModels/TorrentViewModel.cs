using System.Threading.Tasks;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Models.Torrent;
using ReactiveUI;

namespace AvaRaspberry.ViewModels
{
    public class TorrentViewModel : ViewModelBase
    {
        private readonly ITorrentComunicator _communicator;
        private readonly Task _updateTask;
        private TorrentClientStatistic? _torrentClientStatistic;
        private const int GlobalDelay = 0;

        public TorrentViewModel(ITorrentComunicator communicator)
        {
            _communicator = communicator;
            _updateTask = Task.Run(StartUpdate);
        }

        public TorrentClientStatistic TorrentClientStatistic
        {
            get => _torrentClientStatistic ?? new TorrentClientStatistic();
            protected set => this.RaiseAndSetIfChanged(ref _torrentClientStatistic, value);
        }

        private async Task StartUpdate()
        {
            while (true)
            {
                try
                {
                    TorrentClientStatistic = _communicator.GetStatisticData();
                }
                catch
                {
                    // ignored
                }
                finally
                {
                    await Task.Delay(GlobalDelay);
                }
            }
        }
    }
}