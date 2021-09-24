using Newtonsoft.Json;

namespace qBittorrent.qBittorrentApi
{
    public class GeneralProperties
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        // TODO : Unix time
        [JsonProperty("creation_date")]
        public int CreationDate { get; set; }

        [JsonProperty("dl_limit")]
        public int DlLimit { get; set; }

        [JsonProperty("nb_connections")]
        public int NbConnections { get; set; }

        [JsonProperty("nb_connections_limit")]
        public int NbConnectionsLimit { get; set; }

        [JsonProperty("piece_size")]
        public int PieceSize { get; set; }

        [JsonProperty("save_path")]
        public string SavePath { get; set; }

        [JsonProperty("seeding_time")]
        public int SeedingTime { get; set; }

        [JsonProperty("share_ratio")]
        public double ShareRatio { get; set; }

        [JsonProperty("time_elapsed")]
        public int TimeElapsed { get; set; }

        [JsonProperty("total_downloaded")]
        public int TotalDownloaded { get; set; }

        [JsonProperty("total_downloaded_session")]
        public int TotalDownloadedSession { get; set; }

        [JsonProperty("total_uploaded")]
        public int TotalUploaded { get; set; }

        [JsonProperty("total_uploaded_session")]
        public int TotalUploadedSession { get; set; }

        [JsonProperty("total_wasted")]
        public int TotalWasted { get; set; }

        [JsonProperty("up_limit")]
        public int UpLimit { get; set; }

        [JsonProperty("addition_date")]
        public int AdditionDate { get; set; }

        [JsonProperty("completion_date")]
        public int CompletionDate { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("dl_speed")]
        public int DlSpeed { get; set; }

        [JsonProperty("dl_speed_avg")]
        public int DlSpeedAvg { get; set; }

        [JsonProperty("eta")]
        public int Eta { get; set; }

        [JsonProperty("last_seen")]
        public int LastSeen { get; set; }

        [JsonProperty("peers")]
        public int Peers { get; set; }

        [JsonProperty("peers_total")]
        public int PeersTotal { get; set; }

        [JsonProperty("pieces_have")]
        public int PiecesHave { get; set; }

        [JsonProperty("pieces_num")]
        public int PiecesNum { get; set; }

        [JsonProperty("reannounce")]
        public int Reannounce { get; set; }

        [JsonProperty("seeds")]
        public int Seeds { get; set; }

        [JsonProperty("seeds_total")]
        public int SeedsTotal { get; set; }

        [JsonProperty("total_size")]
        public double TotalSize { get; set; }

        [JsonProperty("up_speed")]
        public int UpSpeed { get; set; }

        [JsonProperty("up_speed_avg")]
        public int UpSpeedAvg { get; set; }
    }
}