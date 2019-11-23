using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Configurations
{
    public class SendgridConfiguration
    {
        public string SourceEmail { get; set; }
         
        public string SourceName { get; set; }
        public string ApiKey { get; set; }
        public string UserName { get; set; }
    }
}
