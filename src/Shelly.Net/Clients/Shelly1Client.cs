using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using NrjSolutions.Shelly.Net.Dtos;
using NrjSolutions.Shelly.Net.Options;

namespace NrjSolutions.Shelly.Net.Clients
{
    public class Shelly1Client : ShellyClientBase, IShelly1
    {
        public Shelly1Client(HttpClient httpClient, Shelly1Options shelly1Options) : base(httpClient, shelly1Options)
        {
        }

        public async Task<ShellyResult<Shelly1StatusDto>> GetStatus(CancellationToken cancellationToken, TimeSpan? timeout = null)
        {
            var endpoint = ServerUri.AppendPathSegment("status");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpoint);
            return await ExecuteRequestAsync<Shelly1StatusDto>(requestMessage, cancellationToken, timeout);
        }
    }
}