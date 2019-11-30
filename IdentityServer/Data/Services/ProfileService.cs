using System;
using System.Threading.Tasks;
using IdentityServer.Data.Factory;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Data.Services
{
    public class ProfileService<TUser> : IProfileService where TUser : ApplicationUser
    {

        protected readonly ApplicationUserClaimsPrincipalFactory<ApplicationUser, IdentityRole> ClaimsFactory;
        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger<ProfileService<TUser>> Logger;

        /// <summary>
        /// The user manager.
        /// </summary>
        protected readonly UserManager<TUser> UserManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService{TUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="claimsFactory">The claims factory.</param>
        public ProfileService(UserManager<TUser> userManager,
            ApplicationUserClaimsPrincipalFactory<ApplicationUser, IdentityRole> claimsPrincipalFactory)
        {
            UserManager = userManager;
            ClaimsFactory = claimsPrincipalFactory;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService{TUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="claimsFactory">The claims factory.</param>
        /// <param name="logger">The logger.</param>
        public ProfileService(UserManager<TUser> userManager,
            ApplicationUserClaimsPrincipalFactory<ApplicationUser, IdentityRole> claimsFactory,
            ILogger<ProfileService<TUser>> logger)
        {
            UserManager = userManager;
            ClaimsFactory = claimsFactory;
            Logger = logger;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context.Caller == IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint || context.Caller == "ClaimsProviderAccessToken")
            {
                var sub = context.Subject?.GetSubjectId();
                if (sub == null) throw new Exception("No sub claim present");

                var user = await UserManager.FindByIdAsync(sub);
                if (user == null)
                {
                    Logger?.LogWarning("No user found matching subject Id: {0}", sub);
                }
                else
                {


                    var principal2 = await ClaimsFactory.CreateAsync(user);
                    if (principal2 == null) throw new Exception("ClaimsFactory failed to create a principal");


                    context.AddRequestedClaims(principal2.Claims);
                }
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new Exception("No subject Id claim present");

            var user = await UserManager.FindByIdAsync(sub);
            if (user == null)
            {
                Logger?.LogWarning("No user found matching subject Id: {0}", sub);
            }

            context.IsActive = user != null;

        }

    }
}

