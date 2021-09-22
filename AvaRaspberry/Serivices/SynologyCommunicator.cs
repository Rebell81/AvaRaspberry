using System.Linq;
using System.Threading.Tasks;
using AvaRaspberry.Extenstion;
using AvaRaspberry.Models.Torrent;
using AvaRaspberry.ViewModels;
using qBittorrent.qBittorrentApi;
using SynologyClient;

namespace AvaRaspberry.Serivices
{
    public class SynologyCommunicator : IPcCommunicator
    {
        private SynologyApi _api;

        public SynologyCommunicator()
        {
            Task.Factory.StartNew(() =>
            {
                var config = ConfigurationSingleton.Instance.Widgets.Synology;


                var dd = new AppSettingsClientConfig
                {
                    User = config.User,
                    Password = config.Password,
                    ApiBaseAddressAndPathNoTrailingSlash = $"https://{config.Host}:{config.Port}/webapi"
                };

                var session = new SynologySession(dd);
                session.Login();
                _api = new SynologyApi(session, dd);
            });
        }


        public GetDiskstationUtilizationResponse GetUtilization()
        {
            if (_api is null) return new GetDiskstationUtilizationResponse();

            var info = _api.GetUtilization();
            return info;
        }
        
        public Task<NetworkStatistic> GetNetworkStatisticData()
        {
            return Task.Run(() =>
            {
                if (_api is null) return new NetworkStatistic();

                var info = _api.GetUtilization().Data.Network;
                var total = info.FirstOrDefault();
                return new NetworkStatistic()
                {
                    TotalRx = total?.rx ?? 0,
                    TotalTx = total?.tx ?? 0,
                    Connection_status = connection_status.connected,
                    Device = total?.device
                };
            });
        }

    }
}