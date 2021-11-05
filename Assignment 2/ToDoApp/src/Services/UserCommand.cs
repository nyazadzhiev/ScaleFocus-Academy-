using System;
using System.Collections.Generic;
using System.Text;
using ToDoAppData;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class UserCommand
    {
        private UserService _userService;

        public UserCommand(UserService userService)
        {
            _userService = userService;
        }

        public void PromptLogIn()
        {
            Console.WriteLine("Enter your user name:");
            string userName = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            _userService.Login(userName, password);
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
            _userService.LogOut();
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
            if (String.IsNullOrEmpty(name))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }

            Console.WriteLine("Password:");
            string password = Console.ReadLine();
            if (String.IsNullOrEmpty(password))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }

            Console.WriteLine("First Name:");
            string firstName = Console.ReadLine();
            if (String.IsNullOrEmpty(firstName))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }

            Console.WriteLine("Last Name:");
            string lastName = Console.ReadLine();
            if (String.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }

            bool isAdmin;
            Console.WriteLine("Enter role (admin or user)");
            string answer = Console.ReadLine();
            if (String.IsNullOrEmpty(answer))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }


            if (answer.ToLower() == "admin")
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }

            bool isSuccess = _userService.CreateUser(name, password, firstName, lastName, isAdmin);
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

            _userService.EditUser(username);
        }

        public void PromptDeleteUser()
        {
            Console.WriteLine("Enter username");
            string username = Console.ReadLine();

            _userService.DeleteUser(username);
        }

        public void PromptGetAllUsers()
        {
            List<User> users = _userService.GetAllUsers();

            foreach (User user in users)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(user.ToString());
            }
        }
    }
}
