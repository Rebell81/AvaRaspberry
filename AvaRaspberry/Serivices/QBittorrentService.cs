using AvaRaspberry.Interfaces;


namespace AvaRaspberry.Serivices
{
    public static class QBitTorrentService
    {
        private static INetworkCommunicator _qBitTorrentCommunicatorFalcon;
        private static INetworkCommunicator _qBitTorrentCommunicatorPi;

        private static IUserPassHostPort _configFalcon, _configPi;
        private static bool _inited = false;

        public static INetworkCommunicator InstanceFalcon => GetInstanceFalcon();

        public static  INetworkCommunicator InstancePi => GetInstancePi();

        public static void Init(IUserPassHostPort configFalcon, IUserPassHostPort configPi)
        {
            _configFalcon = configFalcon;
            _configPi = configPi;
            _inited = true;
        }

        private static INetworkCommunicator GetInstanceFalcon()
        {
            if (!_inited)
                throw new System.Exception("Init first");

            return _qBitTorrentCommunicatorFalcon ??= new QBittorrentCommunicator(_configFalcon);
        }

        private static INetworkCommunicator GetInstancePi()
        {
            if (!_inited)
                throw new System.Exception("Init first");

            return _qBitTorrentCommunicatorPi ??= new QBittorrentCommunicator(_configPi);
        }
    }
}
