using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Test;

namespace InnerSpace.IdentityServer.Models
{
    internal class Users
    {
        public static List<TestUser> Get()
        {
            return new List<TestUser> {
            new TestUser {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "admin1",
                Password = "password",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Email, "admin1@innerspace.com"),
                    new Claim(JwtClaimTypes.Role, "admin")
                }
            },
              new TestUser {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DAB1",
                Username = "fieldEngineer1",
                Password = "password",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Email, "fieldEngineer1@innerspace.com"),
                    new Claim(JwtClaimTypes.Role, "fieldEngineer")
                }
            },
                new TestUser {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DAB2",
                Username = "customer1",
                Password = "password",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Email, "customer1@innerspace.com"),
                    new Claim(JwtClaimTypes.Role, "customer")
                }
            }
                ,
                new TestUser {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DAB3",
                Username = "auditor1",
                Password = "password",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Email, "auditor1@innerspace.com"),
                    new Claim(JwtClaimTypes.Role, "auditor")
                }
            }
        };
        }
    }
}
