using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NrjSolutions.Shelly.Net.Dtos;
using RestSharp;
using RestSharp.Authenticators;

namespace NrjSolutions.Shelly.Net
{
    public abstract class ShellyClientBase<T>
    {
        private readonly string _userName;
        private readonly string _password;
        private readonly Uri _server;
        
        protected int DefaultTimeoutMs = 5000;
        
        public ShellyClientBase(string userName, string password, Uri server, TimeSpan? defaultTimeout = null)
        {
            if (userName == null) throw new ArgumentNullException(nameof(userName));
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (server == null) throw new ArgumentNullException(nameof(server));

            _userName = userName;
            _password = password;
            _server = server;

            if (defaultTimeout.HasValue)
            {
                DefaultTimeoutMs = Convert.ToInt32(defaultTimeout.Value.TotalMilliseconds);
            }
            else
            {
                DefaultTimeoutMs = 5000;
            }
        }

        protected async Task<ShellyResult<T>> ExecuteAsync(IRestRequest restRequest, Method method, CancellationToken cancellationToken)
        {
            var client = GetRestClient();
            var response = await client.ExecuteAsync(restRequest, method, cancellationToken);

            if (response.StatusCode == 0)
            {
                // RestSharp will return with a status code of 0 if timeout is reached
                return ShellyResult<T>.Failure(true, "Device did not respond within timeout period");
            }
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                return ShellyResult<T>.Failure(true, "Device responded with ServiceUnavailable");
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return ShellyResult<T>.Success(default, "Device responded with NotFound");
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return ShellyResult<T>.Failure(false, $"Device responded with http status code {response.StatusCode}");
            }
            
            return HandleOkResponse(response);
        }

        protected abstract ShellyResult<T> HandleOkResponse(IRestResponse response);
        
        protected virtual IRestClient GetRestClient()
        {
            IRestClient client = new RestClient(_server);
            client.Authenticator = new HttpBasicAuthenticator(_userName, _password);
            return client;
        }
    }
}