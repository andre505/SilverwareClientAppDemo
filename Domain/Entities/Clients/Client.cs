using Domain.Common;
using Domain.Common.Enums;
using Domain.Entities.Identity;
using Domain.Entities.POSServices;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{ 
    public class Client : AuditableBaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
        [MaxLength(100)]
        public string EmailAddress { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
         [MaxLength(100)]
        public ClientType ClientType { get; set; }
        public virtual ICollection<POSService> POSServices { get; set; }
        public string profileImageUrl { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser AssignedEmployee { get; set; }
    }
}
