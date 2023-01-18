using System;
using System.Threading;
using System.Threading.Tasks;
using NrjSolutions.Shelly.Net.Dtos;

namespace NrjSolutions.Shelly.Net.Clients
{
    public interface IShelly1Pm
    {
        Task<ShellyResult<Shelly1PmStatusDto>> GetStatus(CancellationToken cancellationToken, TimeSpan? timeout = null);
    }
}