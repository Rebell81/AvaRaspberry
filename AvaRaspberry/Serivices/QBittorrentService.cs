using System;
using System.Threading.Tasks;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Models;
using AvaRaspberry.Models.Torrent;
using qBittorrent.qBittorrentApi;

namespace AvaRaspberry.Serivices
{
    public class QBittorrentService : INetworkCommunicator
    {
        private Api _api;

        public QBittorrentService(TorrentConfig config)
        {
            var protocol = config.Ssl ? "https" : "http";
            var creds = new ServerCredential(new Uri($"{protocol}://{config.Host}:{config.Port}"), config.User, config.Password);
            _api = new Api(creds);

        }

        public async Task<NetworkStatistic> GetNetworkStatisticData()
        {
            try
            {
                var data = await _api.GetTransferInfo();
                

                return new NetworkStatistic
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
            catch(Exception ex)
            {
                Logger.Instance.Log(ex);
                return new();
            }
        }
    }
}