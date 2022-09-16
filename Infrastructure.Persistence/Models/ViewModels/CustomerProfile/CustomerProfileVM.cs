using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Models.ViewModels.CustomerProfile
{
    public class CustomerProfileVM
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string DateOfBirth { get; set; }
        public List<CustomerTrustedContactVM> CustomerTrustedContacts { get; set; }
        public CustomerLocationVM CustomerLocation { get; set; }
    }

    public class CustomerTrustedContactVM
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }

    public class CustomerLocationVM
    {
        public CustomerStateVM CustomerState { get; set; }
        public CustomerLGAVM CustomerLGA { get; set; }
        public CustomerDistrictVM CustomerDistrict { get; set; }
    }

    public class CustomerStateVM
    {
        public string StateId { get; set; }
        public string StateName { get; set; }
    }

    public class CustomerLGAVM
    {
        public string LGAId { get; set; }
        public string LGAName { get; set; }
    }
    public class CustomerDistrictVM
    {
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
    }

}
