using Domain.Common;
using Domain.Common.Enums;
using Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.POSServices
{
    public class POSService : AuditableBaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
    }
}
