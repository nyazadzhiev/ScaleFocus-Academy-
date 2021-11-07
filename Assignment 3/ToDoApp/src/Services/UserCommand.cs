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
        private UserInput userInput;

        public UserCommand(UserService userService)
        {
            _userService = userService;
            userInput = new UserInput();
        }

        public void PromptLogIn()
        {
            try
            {
                string userName = userInput.EnterValue("username");

                string password = userInput.EnterValue("password");

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
            catch
            {
                Console.WriteLine("Invalid input");
            }
        }

        public void PromptLogOut()
        {
            _userService.LogOut();
        }

        public void PromptCreateUser()
        {
            try
            {
                string name = userInput.EnterValue("username");

                string password = userInput.EnterValue("password");

                string firstName = userInput.EnterValue("First Name");

                string lastName = userInput.EnterValue("Last Name");

                bool isAdmin = userInput.EnterRole();

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
            catch
            {
                Console.WriteLine("Invalid input");
            }
        }

        public void PromptEditUser()
        {
            try
            {
                string username = userInput.EnterValue("username");

                _userService.EditUser(username);
            }
            catch
            {
                Console.WriteLine("Invalid input");
            }
        }

        public void PromptDeleteUser()
        {
            try
            {
                string username = userInput.EnterValue("username");

                _userService.DeleteUser(username);
            }
            catch
            {
                Console.WriteLine("Invalid input");
            }
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
