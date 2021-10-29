using System;
using System.Collections.Generic;
using ToDoAppEntities;
using ToDoAppServices;

namespace ToDoApp
{
    class Program
    {
        private static UserService userService = new UserService();
        private static Menu menu = new Menu();

        static void Main(string[] args)
        {
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
    }
}
