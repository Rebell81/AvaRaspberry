using System;
using System.Threading.Tasks;
using AvaRaspberry.Extenstion;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Models.Torrent;
using qBittorrent.qBittorrentApi;

namespace AvaRaspberry.Serivices
{
    public class QBittorrentService : ITorrentComunicator
    {
        private Api _api;

        public QBittorrentService()
        {

            var config = ConfigurationSingleton.Instance.Widgets.Qb;

            var creds = new ServerCredential(new Uri($"https://{config.Host}:{config.Port}"), config.User, config.Password);
            _api = new Api(creds);

        }

        public async Task<TorrentClientStatistic> GetStatisticData()
        {
            try
            {
                var data = await _api.GetTransferInfo();
                

                return new TorrentClientStatistic
                {
                    TotalRx = data.dl_info_speed,
                    TotalTx = data.up_info_speed,
                    Connection_status = data.connection_status,
                    DhtNodes = data.dht_nodes,
                    DlRateLimit = data.dl_rate_limit,
                    Queueing = data.queueing,
                    RefreshInterval = data.refresh_interval,
                    TotalRxSession = data.dl_info_data,
                    TotalTxSession = data.up_info_data,
                    UpRateLimit = data.up_rate_limit,
                    UseAltSpeedLimits = data.use_alt_speed_limits
                };
            }
            catch
            {
                return new();
            }
        }
    }
}