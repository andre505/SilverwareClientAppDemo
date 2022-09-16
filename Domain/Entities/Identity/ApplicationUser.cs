using Application.DTOs.Account;
using Domain.Common.Enums;
using Domain.Entities.POSServices;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.FirstTimePromoUsed = false;
            this.CurrentPromoUsed = false;
            this.isActive = false;
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool FirstTimePromoUsed { get; set; }
        public bool isActive { get; set; }
        public bool CurrentPromoUsed { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModified { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public virtual ICollection<Client> AssignedClients { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }

        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}

