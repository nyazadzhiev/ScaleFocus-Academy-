using IdentityServer4.Models;
using IdentityServer4.Validation;
using ProjectManagementApp.BLL;
using ProjectManagementApp.BLL.Contracts;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManagementApp.WEB
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserManager _userManager;

        public PasswordValidator(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            User user = await _userManager.GetUserByUsernameAsync(context.UserName);

            if (user != null)
            {
                bool authResult = await _userManager.ValidateUserCredentials(context.UserName, context.Password);
                if (authResult)
                {
                    List<string> roles = await _userManager.GetUserRolesAsync(user);

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    context.Result = new GrantValidationResult(subject: user.Id, authenticationMethod: "password", claims: claims);
                }
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");
                }

                return;
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");
        }
    }
}

