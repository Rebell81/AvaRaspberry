using AvaRaspberry.Interfaces;
using System.Collections.Generic;

namespace AvaRaspberry.Models
{
    public class Synology : IUserPassHostPort
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public bool Ssl { get; set; }
    }
    public class TorrentConfig : IUserPassHostPort
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public bool Ssl { get; set; }
    }

    public class Torrents
    {
        public TorrentConfig Falcon { get; set; }
        public TorrentConfig Pi { get; set; }
    }

    public class Widgets
    {
        public Synology Synology { get; set; }
        public Torrents Torrents { get; set; }
    }
}