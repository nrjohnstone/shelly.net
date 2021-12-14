using System;
using System.Threading;
using NrjSolutions.Shelly.Net;

namespace Reporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var shellyUser = args[0];
            var shellyPassword = args[1];
            var shellyUri = args[2];
            
            var shelly = new Shelly1Client(shellyUser, shellyPassword, 
                new Uri(shellyUri));

            while (true)
            {
                var response = shelly.GetStatus(CancellationToken.None).GetAwaiter().GetResult();

                Console.WriteLine($"Success: {response.IsSuccess}");
                Console.WriteLine($"Uptime: {response.Value.Uptime}");
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }
    }
}
