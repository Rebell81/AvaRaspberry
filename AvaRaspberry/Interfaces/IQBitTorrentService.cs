namespace AvaRaspberry.Interfaces
{
    public interface IQBitTorrentService
    {
        public INetworkCommunicator InstanceFalcon { get; }
        public INetworkCommunicator InstancePi { get; }

    }
}