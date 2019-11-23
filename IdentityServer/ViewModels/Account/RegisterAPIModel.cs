using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer.ViewModels.Account
{
    public class RegisterAPIModel
    {
        public int AccountId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Initials { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
