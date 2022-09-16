using Application.Features.Clients;
using Application.Interfaces.Repositories;
using Domain.Common.Enums;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ClientRepositoryAsync : GenericRepositoryAsync<Client>, IClientRepositoryAsync
    {
        private readonly DbSet<Client> _clients;
        private readonly ApplicationDbContext _context;
        public ClientRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _clients = dbContext.Set<Client>();
            _context = dbContext;
        }

        public async Task<ClientServicesResponse> GetPartnerClientServices(int ClientId)
        {
            try
            {
                return await (from client in _context.Clients
                              where client.ClientType.Equals(ClientType.IntegrationPartner)
                                   where client.POSServices.Any(c => c.Id == ClientId)
                                   select new ClientServicesResponse
                                   {
                                        ClientId = ClientId,
                                        ClientName = client.Name,
                                        POSServices = client.POSServices.ToList()

                                   }).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }    
        }

        public async Task<ClientServicesResponse> GetCustomerClientServices(int ClientId)
        {
            try
            {
                return await (from client in _context.Clients
                              where client.ClientType.Equals(ClientType.Customer)
                              where client.POSServices.Any(c => c.Id == ClientId)
                              select new ClientServicesResponse
                              {
                                  ClientId = ClientId,
                                  ClientName = client.Name,
                                  ClientType = client.ClientType.ToString(),
                                  POSServices = client.POSServices.ToList()

                              }).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateProfileImage(int clientId, string profileImageUrl)
        {
            var client = await _clients.Where(c => c.Id == clientId).FirstOrDefaultAsync();

            if (client != null)
            {
                client.profileImageUrl = profileImageUrl;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public Task<bool> IsUniqueClientAsync(string ClientName)
        {
            return _clients
                .AllAsync(p => p.Name != ClientName);
        }
    }
}
