using System;
using System.Collections.Generic;
using System.Text;
using ToDoApp.Entities;

namespace ToDoApp.Services
{
    public class UserCommand
    {
        private static UserService userService = new UserService();

        public void PromptLogIn()
        {
            Console.WriteLine("Enter your user name:");
            string userName = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            userService.Login(userName, password);
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Login failed.");
            }
            else
            {
                Console.WriteLine("Login successful.");
            }
        }

        public void PromptLogOut()
        {
            userService.LogOut();
        }

        public void PromptCreateUser()
        {
            if (UserService.CurrentUser == null || !UserService.CurrentUser.IsAdmin)
            {
                Console.WriteLine("You don't have permission to this operation");
                return;
            }

            Console.WriteLine("User Name:");
            string name = Console.ReadLine();

            Console.WriteLine("Password:");
            string password = Console.ReadLine();

            Console.WriteLine("First Name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Last Name:");
            string lastName = Console.ReadLine();

            bool isAdmin;
            Console.WriteLine("Enter role (admin or user)");
            string answer = Console.ReadLine();


            if (answer.ToLower() == "admin")
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }

            bool isSuccess = userService.CreateUser(name, password, firstName, lastName, isAdmin);
            if (isSuccess)
            {
                Console.WriteLine($"User with name '{name}' added");
            }
            else
            {
                Console.WriteLine($"User with name '{name}' already exists");
            }
        }

        public void PromptEditUser()
        {
            Console.WriteLine("Enter username");
            string username = Console.ReadLine();

            userService.EditUser(username);
        }

        public void PromptDeleteUser()
        {
            Console.WriteLine("Enter username");
            string username = Console.ReadLine();

            userService.DeleteUser(username);
        }

        public void PromptGetAllUsers()
        {
            List<User> users = userService.GetAllUsers();

            foreach (User user in users)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(user.ToString());
            }
        }
    }
}
