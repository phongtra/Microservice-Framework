using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using ReactAndIdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReactAndIdentityServer
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            List<Claim> claims;
            if (user.Email == "vittu@perkele.saatana") {
                claims = new List<Claim>
                {
                    new Claim("name", user.Email),
                    new Claim("preferred_username", user.UserName),
                    new Claim(JwtClaimTypes.Role, "ADMIN"),
                    new Claim(JwtClaimTypes.Role, "USER"),
                    new Claim("userType", "ADMIN"),
                };
                
            }
            else
            {
                claims = new List<Claim>
                {
                    new Claim("name", user.Email),
                    new Claim("preferred_username", user.UserName),
                    new Claim(JwtClaimTypes.Role, "USER"),
                    new Claim("userType", "USER"),
                };
            }
            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = (user != null);
        }
    }
}
