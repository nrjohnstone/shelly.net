using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using NrjSolutions.Shelly.Net.Dtos;
using NrjSolutions.Shelly.Net.Options;

namespace NrjSolutions.Shelly.Net.Clients
{
    public class Shelly1PmClient : ShellyClientBase, IShelly1Pm
    {
        public Shelly1PmClient(HttpClient httpClient, Shelly1PmOptions shellyOptions) : base(httpClient, shellyOptions)
        {
        }
    
        public async Task<ShellyResult<Shelly1PmStatusDto>> GetStatus(CancellationToken cancellationToken, TimeSpan? timeout = null)
        {
            var endpoint = ServerUri.AppendPathSegment("status");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpoint);
            return await ExecuteRequestAsync<Shelly1PmStatusDto>(requestMessage, cancellationToken, timeout);
        }
    }
}