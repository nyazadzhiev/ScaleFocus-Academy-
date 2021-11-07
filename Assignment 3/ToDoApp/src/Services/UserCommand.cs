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
        private Validations validations;

        public UserCommand(UserService userService)
        {
            _userService = userService;
            userInput = new UserInput();
            validations = new Validations();
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
            if (UserService.CurrentUser == null || !UserService.CurrentUser.IsAdmin)
            {
                Console.WriteLine("You don't have permission to this operation");
                return;
            }

            try
            {
                string name = userInput.EnterValue("username");

                string password = userInput.EnterValue("password");

                string firstName = userInput.EnterValue("First Name");

                string lastName = userInput.EnterValue("Last Name");

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
