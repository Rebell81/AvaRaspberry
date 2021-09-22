using System;
using System.Configuration;
using AvaRaspberry.Extenstion;
using AvaRaspberry.Interfaces;
using AvaRaspberry.Models.Torrent;
using qBittorrent.qBittorrentApi;

namespace AvaRaspberry.Serivices
{
    public class QBittorrentService : ITorrentComunicator
    {
        private Api _api;
        public QBittorrentService()
        {
            var config = ConfigurationSingleton.Instance.Widgets.Qb;
            var creds = new ServerCredential(new Uri($"https://{config.Host}:{config.Port}"), config.User, config.Port);
             _api = new Api(creds);
        }
        
        public TorrentClientStatistic GetStatisticData()
        {
            return new();
        }
    }
}