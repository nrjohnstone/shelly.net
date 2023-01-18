using System;

namespace NrjSolutions.Shelly.Net.Options
{
    public class Shelly1PmOptions : IShellyCommonOptions
    {
        public Uri ServerUri { get; set; }
        public TimeSpan? DefaultTimeout { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public Shelly1PmOptions()
        {
            DefaultTimeout = null;
        }
    }
}