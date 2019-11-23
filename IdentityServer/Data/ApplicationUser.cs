using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Data
{
    public class ApplicationUser : IdentityUser 
    { 
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string MobilePhone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string  City { get; set; }
        public Guid TenantId { get; set; }

    }
}
