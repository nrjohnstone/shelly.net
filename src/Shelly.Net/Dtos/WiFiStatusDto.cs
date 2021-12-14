using Newtonsoft.Json;

namespace NrjSolutions.Shelly.Net.Dtos
{
    public class WiFiStatusDto
    {
        [JsonProperty("rssi")]
        public int RSSI { get; set; }

        [JsonProperty("ssid")]
        public string SSID { get; set; }

        [JsonProperty("connected")]
        public bool Connected { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }
    }
}
