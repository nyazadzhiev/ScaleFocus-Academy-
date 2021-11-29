using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL
{
    public class DatabaseSeeder
    {
        public static void Seed(DatabaseContext database)
        {
            if (database.Database.EnsureCreated())
            {
                database.Users.Add(new User()
                {
                    Username = "admin",
                    Password = "adminpassword",
                    FirstName = "Admin",
                    LastName = "Admin",
                    IsAdmin = true
                });

                database.SaveChanges();
            }
        }
    }
}
