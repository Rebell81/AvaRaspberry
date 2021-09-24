using System;
using System.Linq;
using System.Threading.Tasks;
using AvaRaspberry.Extenstion;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Models.Torrent;
using qBittorrent.qBittorrentApi;
using SynologyClient;

namespace AvaRaspberry.Serivices.Communicators
{
    public class SynologyCommunicator : IPcCommunicator
    {
        private SynologyApi _api;

        public SynologyCommunicator()
        {
            try
            {
                var config = ConfigurationService.Instance.Widgets.Synology;

                var protocol = config.Ssl ? "https" : "http";

                var dd = new AppSettingsClientConfig
                {
                    User = config.User,
                    Password = config.Password,
                    ApiBaseAddressAndPathNoTrailingSlash = $"{protocol}://{config.Host}:{config.Port}/webapi"
                };

                var session = new SynologySession(dd);
                session.Login();
                _api = new SynologyApi(session, dd);
            }
            catch (Exception ex)
            {
                LoggerService.Instance.Log(ex);
            }
        }


        public GetDiskstationUtilizationResponse GetUtilization()
        {
            if (_api is null) return new GetDiskstationUtilizationResponse();

            var info = _api.GetUtilization();
            return info;
        }

        public Task<NetworkStatisticResponce> GetNetworkStatisticData()
        {
            return Task.Run(() =>
            {
                if (_api is null) return new NetworkStatisticResponce();

                try
                {
                    var info = _api.GetUtilization();
                    if (info != null && info.Data != null)
                    {
                        var total = info.Data.Network.FirstOrDefault();
                        return new NetworkStatisticResponce()
                        {
                            TotalRx = total?.rx ?? 0,
                            TotalTx = total?.tx ?? 0,
                            Connection_status = connection_status.connected,
                            Device = total?.device,
                            Result = true
                        };
                    }

                    return new() { Result = false };
                }
                catch (Exception ex)
                {
                    LoggerService.Instance.Log(ex);
                    return new() { Result = false };
                }

            });
        }

    }
}