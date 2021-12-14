using Newtonsoft.Json;

namespace NrjSolutions.Shelly.Net.Dtos
{
    public class MQTTDto
    {
        [JsonProperty("connected")]
        public bool Connected { get; set; }
    }
}