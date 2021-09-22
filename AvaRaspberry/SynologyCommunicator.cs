using System;
using AvaRaspberry.Extenstion;
using AvaRaspberry.ViewModels;
using SynologyClient;

namespace AvaRaspberry
{
    public class SynologyCommunicator : IPcCommunicator
    {
        private readonly SynologyApi? _api;

        public SynologyCommunicator()
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
        }


        public GetDiskstationUtilizationResponse GetUtilization()
        {
            if (_api is null) return new GetDiskstationUtilizationResponse();

            var info = _api.GetUtilization();
            return info;
        }
    }

    public class PiCommunicator : IPcCommunicator
    {
        public GetDiskstationUtilizationResponse GetUtilization()
        {
            return new GetDiskstationUtilizationResponse();
        }
    }
}