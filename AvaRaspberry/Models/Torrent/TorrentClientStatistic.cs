using ReactiveUI;

namespace AvaRaspberry.Models.Torrent
{
    public class TorrentClientStatistic : Base
    {

        private long _totalRx, _totalTx;
        //bytes
        public long TotalRx
        {
            get => _totalRx;
            set => RaiseAndSetIfChanged(ref _totalRx, value);
        }
        
        //bytes
        public long TotalTx
        {
            get => _totalTx;
            set => RaiseAndSetIfChanged(ref _totalTx, value);
        }
    }
}