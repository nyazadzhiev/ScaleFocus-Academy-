using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL
{
    public class DatabaseSeeder
    {
        public static void Seed(IServiceProvider applicationServices)
        {
            using (IServiceScope serviceScope = applicationServices.CreateScope())
            {
                DatabaseContext database = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                if (database.Database.EnsureCreated())
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    SeedAdmin(database, hasher);
                    SeedManager(database, hasher);

                    database.SaveChanges();
                }
            }
        }

        private static void SeedAdmin(DatabaseContext database, PasswordHasher<User> hasher)
        {
            User admin = new User()
            {
                Id = Guid.NewGuid().ToString("D"),
                Email = "admin@admin.test",
                NormalizedEmail = "admin@admin.test".ToUpper(),
                EmailConfirmed = true,
                UserName = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                NormalizedUserName = "admin".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            admin.PasswordHash = hasher.HashPassword(admin, "adminpassword");

            IdentityRole identityRole = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "Admin",
                NormalizedName = "Admin".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };

            IdentityUserRole<string> identityUserRole = new IdentityUserRole<string>()
            {
                RoleId = identityRole.Id,
                UserId = admin.Id
            };

            database.Roles.Add(identityRole);
            database.Users.Add(admin);
            database.UserRoles.Add(identityUserRole);
        }

        private static void SeedManager(DatabaseContext database, PasswordHasher<User> hasher)
        {
            User manager = new User()
            {
                Id = Guid.NewGuid().ToString("D"),
                Email = "manager@manager.test",
                NormalizedEmail = "manager@manager.test".ToUpper(),
                EmailConfirmed = true,
                UserName = "manager",
                FirstName = "Manager",
                LastName = "Manager",
                NormalizedUserName = "manager".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            manager.PasswordHash = hasher.HashPassword(manager, "managerpass");

            IdentityRole identityRole = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "Manager",
                NormalizedName = "Manager".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };

            IdentityUserRole<string> identityUserRole = new IdentityUserRole<string>()
            {
                RoleId = identityRole.Id,
                UserId = manager.Id
            };

            database.Roles.Add(identityRole);
            database.Users.Add(manager);
            database.UserRoles.Add(identityUserRole);
        }
    }
}
