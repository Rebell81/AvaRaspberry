using Newtonsoft.Json;

namespace qBittorrent.qBittorrentApi
{
    public class TrackersProperties
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("num_peers")]
        public int NumPeers { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}