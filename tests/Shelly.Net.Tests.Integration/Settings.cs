using System;

namespace Shelly.Net.Tests.Integration
{
    internal class Settings
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string ShellyUri { get; set; }

        public static Settings IntegrationTest()
        {
            // Allow overriding defaults with environment variables if required
            var shellyImposterUri = Environment.GetEnvironmentVariable("SHELLY_IMPOSTER_URI", EnvironmentVariableTarget.Process);
            
            if (string.IsNullOrWhiteSpace(shellyImposterUri))
            {
                // Default to initial port in docker-compose
                shellyImposterUri = "http://localhost:8095";
            }
            
            var shellyTestUser = "test-user";  
            var shellyPassword = "test-password";
            
            return new Settings()
            {
                User = shellyTestUser,
                Password = shellyPassword,
                ShellyUri = shellyImposterUri
            };
        }
    }
}