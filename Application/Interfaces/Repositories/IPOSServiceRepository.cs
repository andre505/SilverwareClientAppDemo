using Application.Features.Clients;
using Domain.Entities;
using Domain.Entities.POSServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IPOSServiceRepositoryAsync : IGenericRepositoryAsync<POSService>
    {
        Task<bool> IsUniquePOSServiceAsync(string ServiceName);

        //Get clients that offer a service
        Task<ServiceClientsResponse> GetServiceClients(int ServiceId);
    }
}
