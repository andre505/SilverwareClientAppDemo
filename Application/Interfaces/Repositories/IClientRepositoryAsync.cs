using Application.Features.Clients;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IClientRepositoryAsync : IGenericRepositoryAsync<Client>
    {
        Task<bool> IsUniqueClientAsync(string ClientName);

        //Get services offered by an integration partner 
        Task<ClientServicesResponse> GetPartnerClientServices(int ClientId);

        //Get services subscribed to by a client/customer
        Task<ClientServicesResponse> GetCustomerClientServices(int ClientId);
        Task<bool> UpdateProfileImage(int clientId, string profileImageUrl);

    }
}
