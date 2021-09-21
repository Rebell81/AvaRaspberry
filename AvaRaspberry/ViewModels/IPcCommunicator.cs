using SynologyClient;

namespace AvaRaspberry.ViewModels
{
    public interface IPcCommunicator
    {
        public GetDiskstationUtilizationResponse GetUtilization();
    }
}