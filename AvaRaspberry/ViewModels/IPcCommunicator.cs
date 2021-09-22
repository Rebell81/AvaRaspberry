using AvaRaspberry.Interfaces;
using SynologyClient;

namespace AvaRaspberry.ViewModels
{
    public interface IPcCommunicator : INetworkCommunicator
    {
        public GetDiskstationUtilizationResponse GetUtilization();
    }
}