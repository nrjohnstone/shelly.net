using System;
using System.Threading;
using System.Threading.Tasks;
using NrjSolutions.Shelly.Net.Dtos;

namespace NrjSolutions.Shelly.Net.Clients
{
    public interface IShelly1
    {
        Task<ShellyResult<Shelly1StatusDto>> GetStatus(CancellationToken cancellationToken, TimeSpan? timeout = null);
    }
}