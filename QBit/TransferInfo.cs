using Newtonsoft.Json;

namespace qBittorrent.qBittorrentApi
{

    public enum connection_status { connected , firewalled , disconnected }

    public class RequestResult
    {
        public bool Result { get; set; } = true;
    }


    public class TransferInfo : RequestResult
    {
        [JsonProperty("dl_info_speed")]
        public long dl_info_speed { get; set; }

        [JsonProperty("dl_info_data")]
        public long dl_info_data { get; set; }

        [JsonProperty("up_info_speed")]
        public long up_info_speed { get; set; }

        [JsonProperty("up_info_data")]
        public long up_info_data { get; set; }



        [JsonProperty("dl_rate_limit")]
        public long dl_rate_limit { get; set; }

        [JsonProperty("up_rate_limit")]
        public long up_rate_limit { get; set; }



        [JsonProperty("dht_nodes")]
        public long dht_nodes { get; set; }  
        
        [JsonProperty("queueing")]
        public bool queueing { get; set; }   
        
        [JsonProperty("use_alt_speed_limits")]
        public bool use_alt_speed_limits { get; set; }   
        
        [JsonProperty("refresh_interval")]
        public int refresh_interval { get; set; }

        [JsonProperty("connection_status")]
        public connection_status connection_status { get; set; }
    }

}
