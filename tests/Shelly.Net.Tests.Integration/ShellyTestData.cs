namespace Shelly.Net.Tests.Integration
{
    internal static class ShellyTestData
    {
        /// <summary>
        /// Create a object that maps exactly to a valid response json from the status endpoint of a Shelly1 device
        /// https://shelly-api-docs.shelly.cloud/gen1/#status
        /// </summary>
        /// <returns></returns>
        public static object CreateShelly1StatusJsonResponse()
        {
            var statusResponse = new
            {
                wifi_sta = new
                {
                    connected = true,
                    ssid = "abc-home",
                    ip = "10.1.5.10",
                    rssi = -77
                },
                cloud = new
                {
                    connected = true,
                    enabled = true
                },
                mqtt = new
                {
                    connected = true
                },
                update = new
                {
                    status = "pending",
                    has_update = true,
                    new_version = "20200320-123430/v1.6.2@514044b4",
                    old_version = "20200220-123430/v1.6.1@334044b4"
                },
                time = "21:27",
                unixtime = 1639430855,
                serial = 1234,
                ram_total = 35000,
                ram_free = 6000,
                fs_size = 60000,
                fs_free = 5000,
                has_update = true,
                mac = "DC4F2276E575",
                uptime = 600
            };
            return statusResponse;
        }
        
        public static object CreateShelly1PmStatusJsonResponse()
        {
            var statusResponse = new
            {
                wifi_sta = new
                {
                    connected = true,
                    ssid = "abc-home",
                    ip = "10.1.5.10",
                    rssi = -77
                },
                cloud = new
                {
                    connected = true,
                    enabled = true
                },
                mqtt = new
                {
                    connected = true
                },
                update = new
                {
                    status = "pending",
                    has_update = true,
                    new_version = "20200320-123430/v1.6.2@514044b4",
                    old_version = "20200220-123430/v1.6.1@334044b4"
                },
                time = "21:27",
                unixtime = 1639430855,
                serial = 1234,
                ram_total = 35000,
                ram_free = 6000,
                fs_size = 60000,
                fs_free = 5000,
                has_update = true,
                mac = "DC4F2276E575",
                uptime = 600,
                meters = new[]
                {
                    new
                    {
                        power = 1,
                        overpower = 53.5,
                        is_valid = true,
                        timestamp = 1239430855,
                        counters = new[]
                        {
                            50, 100, 200
                        }
                    }
                }
            };
            return statusResponse;
        }
    }
}