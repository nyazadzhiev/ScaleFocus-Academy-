using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Data.Common;
using System.Data.Entity;
using ToDoAppEntities;

namespace ToDoAppData
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        public override void InitializeDatabase(DatabaseContext context)
        {
            base.InitializeDatabase(context);
        }

        protected override void Seed(DatabaseContext context)
        {
            User defaultUser = new User()
            {
                Username = "admin",
                Password = "adminpassword",
                IsAdmin = true,
                FirstName = "Admin",
                LastName = "Admin",
                CreatedAt = DateTime.Now,
                LastEdited = DateTime.Now
            };

            context.Users.Add(defaultUser); 

            context.SaveChanges();

            defaultUser.Creator = defaultUser;
            defaultUser.Modifier = defaultUser;

            context.SaveChanges();

            base.Seed(context);

        }
    }
}

