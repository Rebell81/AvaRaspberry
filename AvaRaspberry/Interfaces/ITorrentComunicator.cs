using AvaRaspberry.Models.Torrent;
using AvaRaspberry.ViewModels;
using System.Threading.Tasks;

namespace AvaRaspberry.Interfaces
{
    public interface ITorrentComunicator
    {
        public Task<TorrentClientStatistic> GetStatisticData();
    }
}