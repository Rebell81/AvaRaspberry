using SynologyClient;

namespace AvaRaspberry.Interfaces
{
    public interface IPcCommunicator : INetworkCommunicator
    {
        public GetDiskstationUtilizationResponse GetUtilization();
    }
}