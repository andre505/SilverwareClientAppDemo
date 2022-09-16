using Domain.Common.Enums;
using Domain.Entities.Identity;
using Domain.Entities.POSServices;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Clients
{
    public class GetAllClientsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ClientType ClientType { get; set; }
        public string Created { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string profileImageUrl { get; set; }
    }
}
