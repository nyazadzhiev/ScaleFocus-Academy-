using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using ToDoAppData;
using ToDoAppEntities;
using ToDoAppServices;

namespace ToDoApp
{
    public class Program
    {
        private static DatabaseContext database;
        private static Menu menu;

        static async Task Main(string[] args)
        {
            database = InitializeApplication();
            menu = new Menu(database);

            bool shouldExit = false;
            while (!shouldExit)
            {
                shouldExit = await menu.MainMenu(UserService.CurrentUser);
            }
        }

        static DatabaseContext InitializeApplication()
        {
            // Read config file
            var _configuration = ConfigInitializer.InitConfig();

            string connectionString = _configuration.GetConnectionString("Default");

            // Create new database and tables 
            Database.SetInitializer(new DatabaseInitializer());

            DatabaseContext database = new DatabaseContext(connectionString);

            return database;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             