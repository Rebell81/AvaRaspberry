using qBittorrent.qBittorrentApi;

namespace AvaRaspberry.Models.Torrent
{
    public class NetworkStatistic : Base
    {

        private long _totalRx, _totalTx, _totalRxSession, _totalTxSession, _dl_rate_limit,
            _up_rate_limit, _dht_nodes, _refresh_interval;

        private bool _queueing, _use_alt_speed_limits;
        private string _device;
        private connection_status _connection_status;

        public string Device
        {
            get => _device;
            set => RaiseAndSetIfChanged(ref _device, value);
        }
        
        /// <summary>
        /// Global download rate (bytes/s)
        /// </summary>
        public long TotalRx
        {
            get => _totalRx;
            set => RaiseAndSetIfChanged(ref _totalRx, value);
        }

        /// <summary>
        /// Global upload rate (bytes/s)
        /// </summary>
        public long TotalTx
        {
            get => _totalTx;
            set => RaiseAndSetIfChanged(ref _totalTx, value);
        }

        /// <summary>
        /// Data uploaded this session (bytes)
        /// </summary>
        public long TotalTxSession
        {
            get => _totalTxSession;
            set => RaiseAndSetIfChanged(ref _totalTxSession, value);
        }

        /// <summary>
        /// Data downloaded this session (bytes)
        /// </summary>
        public long TotalRxSession
        {
            get => _totalRxSession;
            set => RaiseAndSetIfChanged(ref _totalRxSession, value);
        }

        /// <summary>
        /// Download rate limit (bytes/s)
        /// </summary>
        public long DlRateLimit
        {
            get => _dl_rate_limit;
            set => RaiseAndSetIfChanged(ref _dl_rate_limit, value);
        }

        /// <summary>
        /// Upload rate limit (bytes/s)
        /// </summary>
        public long UpRateLimit
        {
            get => _up_rate_limit;
            set => RaiseAndSetIfChanged(ref _up_rate_limit, value);
        }

        /// <summary>
        /// DHT nodes connected to
        /// </summary>
        public long DhtNodes
        {
            get => _dht_nodes;
            set => RaiseAndSetIfChanged(ref _dht_nodes, value);
        }

        /// <summary>
        /// True if torrent queueing is enabled
        /// </summary>
        public bool Queueing
        {
            get => _queueing;
            set => RaiseAndSetIfChanged(ref _queueing, value);
        }

        /// <summary>
        /// True if alternative speed limits are enabled
        /// </summary>
        public bool UseAltSpeedLimits
        {
            get => _use_alt_speed_limits;
            set => RaiseAndSetIfChanged(ref _use_alt_speed_limits, value);
        }

        /// <summary>
        /// Transfer list refresh interval (milliseconds)
        /// </summary>
        public long RefreshInterval
        {
            get => _totalRxSession;
            set => RaiseAndSetIfChanged(ref _refresh_interval, value);
        }

        /// <summary>
        /// Connection status. See possible values here below
        /// </summary>
        public connection_status Connection_status
        {
            get => _connection_status;
            set => RaiseAndSetIfChanged(ref _connection_status, value);
        }

    }
}