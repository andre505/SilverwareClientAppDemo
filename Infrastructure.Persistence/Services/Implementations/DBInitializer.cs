using Domain.Common.Enums;
using Domain.Entities;
using Domain.Entities.POSServices;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Infrastructure.Persistence.Services
{
    public class DbInitializer : IDbInitializer
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
        }

        public void Initialize()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
      

        public void SeedDefaultEntities()
        {
            //Default Alert Levels
            try
            {

                using (var serviceScope = _scopeFactory.CreateScope())
                {
                    using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                    {

                        #region seed test entities 
                        //Clients
                        if (!(context.Clients.Any()))
                        {
                            IList<Client> Clients = new List<Client>() {
                            new Client() { Name = "Cuboh", Description = "Online Ordering Company", ClientType = ClientType.IntegrationPartner, CreatedBy="Admin", Created = DateTime.UtcNow.AddHours(-4)},
                            new Client() { Name = "3C Payment", Description = "Payment Processing Company", ClientType = ClientType.IntegrationPartner, CreatedBy="Admin", Created = DateTime.UtcNow.AddHours(-4)},
                            new Client() { Name = "The Ritz, London", Description = "5 star hotel", ClientType = ClientType.Customer, CreatedBy="Admin", Created = DateTime.UtcNow.AddHours(-4)},
                            };

                            context.Clients.AddRange(Clients);
                        }

                        //PoS Services
                        if (!(context.POSServices.Any()))
                        {
                            IList<POSService> pOSServices = new List<POSService>() {
                            new POSService() { ServiceName = "Payment", ServiceDescription = "Online Ordering", CreatedBy="Admin", Created = DateTime.UtcNow.AddHours(-4)},
                            new POSService() { ServiceName = "InventoryControl", ServiceDescription = "Inventory Control Services", CreatedBy="Admin", Created = DateTime.UtcNow.AddHours(-4)},
                            };
                            context.POSServices.AddRange(pOSServices);
                        }

                        context.SaveChanges();
                        #endregion seed default entities
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + $": {ex.InnerException.Message}");
            }
        }
    }
}
