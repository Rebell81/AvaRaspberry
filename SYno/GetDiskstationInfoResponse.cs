using Newtonsoft.Json;
using System.Collections.Generic;

namespace SynologyClient
{
    public class GetDiskstationInfoResponse : BaseSynologyResponse
    {
        public InfoGetInfo Data { get; set; }
    }

    public class GetDiskstationUtilizationResponse : BaseSynologyResponse
    {
        public InfoGetUtilization Data { get; set; }
    }

    public class InfoGetUtilization
    {
        public Cpu cpu { get; set; }
        public Memory Memory { get; set; }
        public List<Network> Network { get; set; }
    }


    public class Network
    {
        public string device { get; set; }
        public long rx { get; set; }
        public long tx { get; set; }
    }

    public class Memory
    {
        public long avail_real { get; set; }
        public long avail_swap { get; set; }
        public long buffer { get; set; }
        public long cached { get; set; }
        public string device { get; set; }
        public long memory_size { get; set; }
        public long real_usage { get; set; }
        public long si_disk { get; set; }
        public long so_disk { get; set; }
        public long swap_usage { get; set; }
        public long total_real { get; set; }
        public long total_swap { get; set; }
    }
    public class Cpu
    {
        [JsonProperty("15min_load")]
        public long _15min_load { get; set; }


        [JsonProperty("1min_load")]
        public long _1min_load { get; set; }

        [JsonProperty("5min_load")]
        public long _5min_load { get; set; }

        public string device { get; set; }
        public long other_load { get; set; }
        public long system_load { get; set; }
        public long user_load { get; set; }
    }
}