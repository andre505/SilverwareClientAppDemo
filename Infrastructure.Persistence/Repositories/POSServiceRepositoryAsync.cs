using Application.Features.Clients;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Entities.POSServices;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using Domain.Common.Enums;

namespace Infrastructure.Persistence.Repositories
{
    public class POSServiceRepositoryAsync : GenericRepositoryAsync<POSService>, IPOSServiceRepositoryAsync
    {
        private readonly DbSet<POSService> _posService;
        private readonly ApplicationDbContext _context;

        public POSServiceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _posService = dbContext.Set<POSService>();
            _context = dbContext;
        }

        public async Task<ServiceClientsResponse> GetServiceClients(int ServiceId)
        {
            try
            {
                return await (from posService in _context.POSServices
                             where posService.Clients.Any(c => c.Id == ServiceId)
                             where posService.Clients.Any(c => c.ClientType == ClientType.Customer)
                              select new ServiceClientsResponse
                              {
                                 ServiceId  = ServiceId,
                                 ServiceName = posService.ServiceName,
                                 ServiceDescription = posService.ServiceDescription,
                                 Clients = posService.Clients.ToList()
                              }).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Task<bool> IsUniquePOSServiceAsync(string ServiceName)
        {
            return _posService
                .AllAsync(p => p.ServiceName != ServiceName);
        }
    }
}
