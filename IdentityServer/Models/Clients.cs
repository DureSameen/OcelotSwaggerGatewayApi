using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;

namespace InnerSpace.IdentityServer.Models
{
    internal class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client> {
             
            new Client {
    ClientId = "dev-user-api",
    ClientName = "User api",
    AllowAccessTokensViaBrowser= true,
    AllowedGrantTypes = GrantTypes.Implicit,
    AllowedScopes = new List<string>
    {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        IdentityServerConstants.StandardScopes.Email,
         IdentityServerConstants.StandardScopes.OfflineAccess,
        "role", 
         "user_read_only"
    },
    RedirectUris = new List<string> {"http://localhost:59714/oauth2-redirect.html","http://localhost:52792/oauth2-redirect.html"},
    PostLogoutRedirectUris = new List<string> { "http://localhost:59714/" }

},
            new Client {
    ClientId = "dev-catalog-api",
    ClientName = "Catalog api",
    AllowAccessTokensViaBrowser= true,
    AllowedGrantTypes = GrantTypes.Implicit,
    AllowedScopes = new List<string>
    {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        IdentityServerConstants.StandardScopes.Email,
         IdentityServerConstants.StandardScopes.OfflineAccess,
        "role", 
        "catalog_read_only" 
         
    },
    RedirectUris = new List<string> {"http://localhost:59714/oauth2-redirect.html","http://localhost:52791/oauth2-redirect.html"},
    PostLogoutRedirectUris = new List<string> { "http://localhost:59714/" }
},
            new Client {
    ClientId = "dev-ledger-api",
    ClientName = "Ledger api",
    AllowAccessTokensViaBrowser= true,
    AllowedGrantTypes = GrantTypes.Implicit,
    AllowedScopes = new List<string>
    {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        IdentityServerConstants.StandardScopes.Email,
         IdentityServerConstants.StandardScopes.OfflineAccess,
        "role",
         "ledger_read_only" 
       
    },
    RedirectUris = new List<string> {"http://localhost:59714/oauth2-redirect.html","http://localhost:52790/oauth2-redirect.html"},
    PostLogoutRedirectUris = new List<string> { "http://localhost:59714/" }
}
            ,
               new Client {
    ClientId = "dev-gateway-api",
    ClientName = "Gateway api",
    AllowAccessTokensViaBrowser= true,
    AllowedGrantTypes = GrantTypes.Implicit,
    AllowedScopes = new List<string>
    {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        IdentityServerConstants.StandardScopes.Email,
         IdentityServerConstants.StandardScopes.OfflineAccess,
        "role",
        "ledger_read_only",
        "catalog_read_only",
         "user_read_only"
    },
    RedirectUris = new List<string> {"http://localhost:59714/swagger/oauth2-redirect.html"},
    PostLogoutRedirectUris = new List<string> { "http://localhost:59714/" }
}
        };
        }
    }
}
