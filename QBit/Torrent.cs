using Newtonsoft.Json;

namespace qBittorrent.qBittorrentApi
{
    public class Torrent
    {
        [JsonProperty("dlspeed")]
        public int DownloadSpeed { get; set; }

        [JsonProperty("eta")]
        public int Eta { get; set; }

        [JsonProperty("f_l_piece_prio")]
        public bool FirstLastPiecePrioritized { get; set; }

        [JsonProperty("force_start")]
        public bool ForceStart { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("num_complete")]
        public int SeedersInSwarm { get; set; }

        [JsonProperty("num_incomplete")]
        public int LeechersInSwarm { get; set; }

        [JsonProperty("num_leechs")]
        public int LeechersConnected { get; set; }

        [JsonProperty("num_seeds")]
        public int SeedersConnected { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("progress")]
        public double Progress { get; set; }

        [JsonProperty("ratio")]
        public double Ratio { get; set; }

        [JsonProperty("seq_dl")]
        public bool SequentialDownload { get; set; }

        [JsonProperty("size")]
        public double Size { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("super_seeding")]
        public bool SuperSeeding { get; set; }

        [JsonProperty("upspeed")]
        public int UploadSpeed { get; set; }
    }
}