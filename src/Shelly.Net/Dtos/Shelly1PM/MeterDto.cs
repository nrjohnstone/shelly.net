﻿using Newtonsoft.Json;

namespace NrjSolutions.Shelly.Net.Dtos.Shelly1PM
{
    public class MeterDto
    {
        [JsonProperty("power")]
        public double Power { get; set; }

        [JsonProperty("overpower")]
        public double OverPower { get; set; }
        
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        [JsonProperty("timestamp")]
        public long TimeStamp { get; set; }
        
        [JsonProperty("counters")]
        public long[] Counters { get; set; }
    }
}