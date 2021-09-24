using Newtonsoft.Json;

namespace qBittorrent.qBittorrentApi
{
    public class FilesProperties
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("size")]
        public double Size { get; set; }

        [JsonProperty("progress")]
        public float Progress { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("is_seed")]
        public bool IsSeed { get; set; }
    }
}