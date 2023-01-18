using System;
using System.Net.Http;
using System.Threading;
using NrjSolutions.Shelly.Net.Clients;
using NrjSolutions.Shelly.Net.Options;

namespace Reporter
{
    /// <summary>
    /// Test harness for exercising the SDK against real Shelly devices during development
    /// </summary>
    public static class Program
    {
        static void Main(string[] args)
        {
            var shellyUser = args[0];
            var shellyPassword = args[1];
            var shellyUri = args[2];

            Shelly1PmOptions options = new Shelly1PmOptions()
            {
                UserName = shellyUser,
                Password = shellyPassword,
                ServerUri = new Uri(shellyUri)
            };
            
            var shelly = new Shelly1PmClient(new HttpClient(), options);

            while (true)
            {
                var response = shelly.GetStatus(CancellationToken.None).GetAwaiter().GetResult();

                Console.WriteLine($"Success: {response.IsSuccess}");
                Console.WriteLine($"Uptime: {response.Value.Uptime}");
                Console.WriteLine($"Meter Power: {response.Value.Meters[0].Power}");
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }
    }
}
