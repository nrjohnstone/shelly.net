using Newtonsoft.Json;
using NrjSolutions.Shelly.Net.Dtos.Shelly1PM;

namespace NrjSolutions.Shelly.Net.Dtos
{
    /// <summary>
    /// Extended information for the Shelly1PM
    /// https://shelly-api-docs.shelly.cloud/gen1/#shelly1-1pm-status
    /// </summary>
    public class Shelly1PmStatusDto : Shelly1StatusDto
    {
        [JsonProperty("meters")]
        public MeterDto[] Meters { get; set; }
        
        [JsonProperty("relays")]
        public RelayDto[] Relays { get; set; }
    }
}