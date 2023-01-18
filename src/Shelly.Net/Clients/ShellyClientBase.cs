using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NrjSolutions.Shelly.Net.Options;

namespace NrjSolutions.Shelly.Net.Clients
{
    public abstract class ShellyClientBase
    {
        protected readonly HttpClient ShellyHttpClient;
        private readonly IShellyCommonOptions _shellyCommonOptions;
        protected readonly Uri ServerUri;
        
        protected TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);
        
        public ShellyClientBase(HttpClient httpClient, IShellyCommonOptions shellyCommonOptions)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (shellyCommonOptions == null) throw new ArgumentNullException(nameof(shellyCommonOptions));
            

            ShellyHttpClient = httpClient;
            _shellyCommonOptions = shellyCommonOptions;
            ServerUri = shellyCommonOptions.ServerUri;
            
            if (shellyCommonOptions.DefaultTimeout.HasValue)
            {
                DefaultTimeout = shellyCommonOptions.DefaultTimeout.Value;
            }
        }

        protected async Task<ShellyResult<T>> ExecuteRequestAsync<T>(HttpRequestMessage httpRequestMessage,
            CancellationToken cancellationToken, TimeSpan? timeout = null)
        {
            timeout = timeout ?? DefaultTimeout;
            
            using (var timeoutTokenSource = new CancellationTokenSource(timeout.Value))
            {
                var authenticationString = $"{_shellyCommonOptions.UserName}:{_shellyCommonOptions.Password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
                
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                
                var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(timeoutTokenSource.Token , cancellationToken);
                var response = await ShellyHttpClient.SendAsync(httpRequestMessage, linkedTokenSource.Token);
                
                if (response.StatusCode == 0)
                {
                    // Status code of 0 means timeout reached
                    return ShellyResult<T>.TransientFailure("Device did not respond within timeout period");
                }

                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    return ShellyResult<T>.TransientFailure("Device responded with ServiceUnavailable");
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return ShellyResult<T>.Success(default, "Device responded with NotFound");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return ShellyResult<T>.Failure($"Device responded with http status code {response.StatusCode}");
                }

                return await HandleOkResponse<T>(response);
            }
        }

        protected virtual async Task<ShellyResult<T>> HandleOkResponse<T>(HttpResponseMessage response)
        {
            var readAsStringAsync = await response.Content.ReadAsStringAsync();
            var shelly1Status = JsonConvert.DeserializeObject<T>(readAsStringAsync);
            return ShellyResult<T>.Success(shelly1Status);
        }
    }
}