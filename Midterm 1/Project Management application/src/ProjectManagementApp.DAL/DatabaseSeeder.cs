using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL
{
    public class DatabaseSeeder
    {
        public static async Task Seed(DatabaseContext database)
        {
            if (await database.Database.EnsureCreatedAsync())
            {
                await database.Users.AddAsync(new User()
                {
                    Username = "admin",
                    Password = "adminpassword",
                    FirstName = "Admin",
                    LastName = "Admin",
                    IsAdmin = true
                });

                await database.SaveChangesAsync();
            }
        }
    }
}
