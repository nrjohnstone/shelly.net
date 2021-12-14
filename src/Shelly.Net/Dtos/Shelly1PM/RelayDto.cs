using Newtonsoft.Json;

namespace NrjSolutions.Shelly.Net.Dtos.Shelly1PM
{
    public class RelayDto
    {
        [JsonProperty("ison")]
        public bool IsOn { get; set; }

        [JsonProperty("has_timer")]
        public bool HasTimer { get; set; }

        [JsonProperty("timer_remaining")]
        public long TimerRemaining { get; set; }
    }
}