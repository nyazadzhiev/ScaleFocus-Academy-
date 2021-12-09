using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManagementApp.WEB
{
    public class IdentityConfig
    {
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "projectmanapp",
                    AllowOfflineAccess = true,

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes = { "users", "offline_access", "projectmanapp", "roles" }
                }
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResource("roles", new[] { "role" })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                    new ApiScope("users", "My API", new string[]{ ClaimTypes.Name, ClaimTypes.Role }),
                    new ApiScope("offline_access", "RefereshToken"),
                    new ApiScope("projectmanapp", "app")
            };
    }
}
