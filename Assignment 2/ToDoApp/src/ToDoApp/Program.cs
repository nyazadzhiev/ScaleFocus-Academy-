using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using ToDoAppData;
using ToDoAppEntities;
using ToDoAppServices;

namespace ToDoApp
{
    public class Program
    {
        private static Database database;
        private static UserService userService;
        private static Menu menu;

        static void Main(string[] args)
        {
            database = InitializeApplication();
            userService = new UserService(database);
            menu = new Menu(database);

            if (args.Length > 0)
            {
                userService.CreateUser("admin", "adminpassword", "Admin", "Admin", true);
            }
            bool shouldExit = false;
            while (!shouldExit)
            {
                shouldExit = menu.MainMenu(UserService.CurrentUser);
            }
        }

        static Database InitializeApplication()
        {
            // Read config file
            var _configuration = ConfigInitializer.InitConfig();

            string connectionString = _configuration.GetConnectionString("Default");

            // Create new database and tables 
            //DatabaseInitilizer.InitilizeDatabase(connectionString);

            Database database = new Database(connectionString);

            return database;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             