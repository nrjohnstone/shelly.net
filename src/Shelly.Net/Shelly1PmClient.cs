using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NrjSolutions.Shelly.Net.Dtos;
using RestSharp;

namespace NrjSolutions.Shelly.Net
{
    public class Shelly1PmClient : ShellyClientBase<Shelly1PmStatusDto>, IShelly1Pm
    {
        public Shelly1PmClient(string userName, string password, Uri server) : base(userName, password, server)
        {
       
        }
    
        public async Task<ShellyResult<Shelly1PmStatusDto>> GetStatus(CancellationToken cancellationToken, TimeSpan? timeout = null)
        {
            int timeoutMs = timeout.HasValue ? Convert.ToInt32(timeout.Value.TotalMilliseconds) : DefaultTimeoutMs;
            var restRequest = new RestRequest("status") { Timeout = timeoutMs };
            return await ExecuteAsync(restRequest, Method.GET, cancellationToken);
        }

        protected override ShellyResult<Shelly1PmStatusDto> HandleOkResponse(IRestResponse response)
        {
            var shelly1Status = JsonConvert.DeserializeObject<Shelly1PmStatusDto>(response.Content);
            return ShellyResult<Shelly1PmStatusDto>.Success(shelly1Status);
        }
    }
}