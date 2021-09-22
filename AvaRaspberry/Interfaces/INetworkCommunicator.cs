using AvaRaspberry.Models.Torrent;
using System.Threading.Tasks;

namespace AvaRaspberry.Interfaces
{
    public interface INetworkCommunicator
    {
        public Task<NetworkStatistic> GetNetworkStatisticData();
    }
}