using AvaRaspberry.Serivices;
using ReactiveUI;

namespace AvaRaspberry.ViewModels
{
    public class GridWidgetViewModel : ViewModelBase
    {
        private TorrentViewModel _qbTorrentViewModel;

        public TorrentViewModel QbTorrentViewModel
        {
            get => _qbTorrentViewModel;
            protected set => this.RaiseAndSetIfChanged(ref _qbTorrentViewModel, value);
        }

        public GridWidgetViewModel()
        {
            _qbTorrentViewModel = new TorrentViewModel(new QBittorrentService(), "QBittorrent Falcon");
        }

    }
}