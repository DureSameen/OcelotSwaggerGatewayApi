using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityServer.Data.Factory
{
    public class ApplicationUserClaimsPrincipalFactory<TUser, TRole> : IUserClaimsPrincipalFactory<TUser> where TUser : ApplicationUser where TRole : IdentityRole
    {
        /// <summary>   Options for controlling the operation. </summary>
        private readonly IdentityOptions _options;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>   Constructor. </summary>
        /// <remarks>   Werner Teunissen, 02/05/2017. </remarks>
        /// <param name="userManager">      Manager for user. </param>
        /// <param name="roleManager">      Manager for role. </param>
        /// <param name="optionsAccessor">  The options accessor. </param>
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor)
        {
            _userManager = userManager;
             _options = optionsAccessor.Value;
        }

        /// <summary>
        /// create user principal instance
        /// </summary>
        /// <param name="user"></param>

        /// <returns></returns>
        public async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            var principal = await CreateAsyncEx(user);
            var identity = principal.Identities.FirstOrDefault();
            if (null == identity) return await Task.FromResult<ClaimsPrincipal>(null);

            var username = await _userManager.GetUserNameAsync(user);
            var usernameClaim = identity?.FindFirst(claim => claim.Type == _options.ClaimsIdentity.UserNameClaimType && claim.Value == username);
            if (usernameClaim != null)
            {
                identity.RemoveClaim(usernameClaim);
                identity.AddClaim(new Claim(JwtClaimTypes.PreferredUserName , username));
            }
            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Name))
            {
                identity.AddClaim(new Claim(JwtClaimTypes.Name, username));
            }

            var id = await _userManager.GetUserIdAsync(user);
            var idClaim =
                identity.FindFirst(claim => claim.Type == _options.ClaimsIdentity.UserIdClaimType && claim.Value == id);
            if (idClaim != null)
            {
                identity.RemoveClaim(idClaim);
                identity.AddClaim(new Claim(JwtClaimTypes.Id, id));
            }
            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Id))
                identity.AddClaim(new Claim(JwtClaimTypes.Id, id));

            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Subject))
                identity.AddClaim(new Claim(JwtClaimTypes.Subject, id));
            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.FamilyName))
                identity.AddClaim(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.GivenName))
                identity.AddClaim(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.AuthenticationMethod))
                identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationMethod, "pwd")); 

            if (_userManager.SupportsUserEmail)
            {
                var email = await _userManager.GetEmailAsync(user);
                if (!string.IsNullOrWhiteSpace(email))
                {
                    identity.AddClaims(new[]
                    {
                        new Claim(JwtClaimTypes.Email, email),
                        new Claim(JwtClaimTypes.EmailVerified,
                            await _userManager.IsEmailConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
                    });
                }
            }

            if (_userManager.SupportsUserPhoneNumber)
            {
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (!string.IsNullOrWhiteSpace(phoneNumber))
                {
                    identity.AddClaims(new[]
                    {
                        new Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
                        new Claim(JwtClaimTypes.PhoneNumberVerified,
                            await _userManager.IsPhoneNumberConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
                    });
                }
            }

          
          
            identity.AddClaim(new Claim(JwtClaimTypes.IdentityProvider, "idsrv"));
            identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationTime, DateTime.UtcNow.ToEpochTime().ToString(), ClaimValueTypes.Integer));
           
            var displayTenantId = RegisterClaims.TenantId.GetAttribute<DisplayAttribute>();
            

            if (!identity.HasClaim(x => x.Type == displayTenantId.Name))
            {

                identity.AddClaim(
                  new Claim(displayTenantId.Name, user.TenantId.ToString(), ClaimValueTypes.String));
            }


            
            return principal;
        }


        // <summary>

        /// <summary>   Creates a <see cref="ClaimsPrincipal"/> from an user asynchronously. </summary>
        /// <remarks>   Werner Teunissen, 02/05/2017. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        /// <param name="user"> The user to create a <see cref="ClaimsPrincipal"/> from. </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous creation operation, containing the
        /// created <see cref="ClaimsPrincipal"/>.
        /// </returns>
        private async Task<ClaimsPrincipal> CreateAsyncEx(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var userId = await _userManager.GetUserIdAsync(user);
            var userName = await _userManager.GetUserNameAsync(user);

            var id = new ClaimsIdentity("idsrv",
                _options.ClaimsIdentity.UserNameClaimType,
                _options.ClaimsIdentity.RoleClaimType);
            id.AddClaim(new Claim(_options.ClaimsIdentity.UserIdClaimType, userId));
            id.AddClaim(new Claim(_options.ClaimsIdentity.UserNameClaimType, userName));
            if (_userManager.SupportsUserSecurityStamp)
            {
                id.AddClaim(new Claim(_options.ClaimsIdentity.SecurityStampClaimType, await _userManager.GetSecurityStampAsync(user)));
            }

            if (_userManager.SupportsUserClaim)
            {
                id.AddClaims(await _userManager.GetClaimsAsync(user));
            }
            return new ClaimsPrincipal(id);

        }
    }
}
