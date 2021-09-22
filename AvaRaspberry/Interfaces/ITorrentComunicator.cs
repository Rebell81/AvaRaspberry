using AvaRaspberry.Models.Torrent;

namespace AvaRaspberry.Interfaces
{
    public interface ITorrentComunicator
    {
        public TorrentClientStatistic GetStatisticData();
    }
}