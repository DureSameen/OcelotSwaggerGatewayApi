using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace InnerSpace.IdentityServer.Models
{
    internal class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(), 
            new IdentityResource {
                Name = "role",
                UserClaims = new List<string> {"role"}
            }
        };
        }


        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {
            new ApiResource {
                Name = "UserApi",
                DisplayName = "User API",
                Description = "User API",
                UserClaims = new List<string> {"role"},
              //  ApiSecrets = new List<Secret> {new Secret("scopeSecret".Sha256())},
                Scopes = new List<Scope> {
                    new Scope("user_read_only"),
                     
                }
            },

               new ApiResource {
                Name = "CatalogApi",
                DisplayName = "Catalog API",
                Description = "Catalog API",
                UserClaims = new List<string> {"role"},
                ApiSecrets = new List<Secret> {new Secret("scopeSecret".Sha256())},
                Scopes = new List<Scope> {
                    new Scope("catalog_read_only"),

                }
            }
               ,
                  new ApiResource {
                Name = "LedgerApi",
                DisplayName = "Ledger API",
                Description = "Ledger API",
                UserClaims = new List<string> {"role"},
                ApiSecrets = new List<Secret> {new Secret("scopeSecret".Sha256())},
                Scopes = new List<Scope> {
                    new Scope("ledger_read_only"),

                }
            },
                     new ApiResource {
                Name = "GatewayApi",
                DisplayName = "Gateway API",
                Description = "Gateway API",
                UserClaims = new List<string> {"role"},
                ApiSecrets = new List<Secret> {new Secret("scopeSecret".Sha256())},
                Scopes = new List<Scope> {
                    new Scope("gateway_read_only"),

                }
            }
        };
        }
    }
}
