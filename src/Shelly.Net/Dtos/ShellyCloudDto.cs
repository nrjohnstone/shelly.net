using Newtonsoft.Json;

namespace NrjSolutions.Shelly.Net.Dtos
{
    public class ShellyCloudDto
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("connected")]
        public bool Connected { get; set; }
    }
}