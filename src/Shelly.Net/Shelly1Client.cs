using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using NrjSolutions.Shelly.Net.Dtos;

namespace NrjSolutions.Shelly.Net
{
    public class Shelly1Client : ShellyClientBase, IShelly1
    {
        public Shelly1Client(string userName, string password, HttpClient httpClient, Uri serverUri) : base(userName, password, httpClient, serverUri)
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