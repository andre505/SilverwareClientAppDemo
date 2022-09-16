using Domain.Entities.POSServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients
{
    public class ClientServicesResponse
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientType { get; set; }
        public List<POSService> POSServices { get; set; }
    }
}
