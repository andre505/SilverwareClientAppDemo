using Domain.Entities;
using Domain.Entities.POSServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients
{
    public class ServiceClientsResponse
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public List<Client> Clients { get; set; }
    }

    public class ClientResponse
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientDescription { get; set; }
        public string ClientType { get; set; }
    }
}
