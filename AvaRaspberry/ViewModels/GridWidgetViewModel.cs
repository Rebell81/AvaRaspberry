using AvaRaspberry.Extenstion;
using AvaRaspberry.Serivices;
using ReactiveUI;
using System.Linq;

namespace AvaRaspberry.ViewModels
{
    public class GridWidgetViewModel : ViewModelBase
    {
        private TorrentViewModel _qbTorrentViewModel, _qbTorrentViewModelLocal;

        public TorrentViewModel QbTorrentViewModel
        {
            get => _qbTorrentViewModel;
            protected set => this.RaiseAndSetIfChanged(ref _qbTorrentViewModel, value);
        }

        public TorrentViewModel QbTorrentViewModelLocal
        {
            get => _qbTorrentViewModelLocal;
            protected set => this.RaiseAndSetIfChanged(ref _qbTorrentViewModelLocal, value);
        }

        public GridWidgetViewModel()
        {
            _qbTorrentViewModel = new TorrentViewModel(new QBittorrentService(ConfigurationSingleton.Instance.Widgets.Torrents.First()), "QBittorrent Falcon");
            _qbTorrentViewModelLocal = new TorrentViewModel(new QBittorrentService(ConfigurationSingleton.Instance.Widgets.Torrents.Last()), "QBittorrent Pi");
        }

    }
}