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

            string name = userInput.EnterValue("username");
            if (String.IsNullOrEmpty(name))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }

            string password = userInput.EnterValue("password");
            if (String.IsNullOrEmpty(password))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }

            string firstName = userInput.EnterValue("First Name");
            if (String.IsNullOrEmpty(firstName))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }

            string lastName = userInput.EnterValue("Last Name");
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
            string username = userInput.EnterValue("username");

            _userService.EditUser(username);
        }

        public void PromptDeleteUser()
        {
            string username = userInput.EnterValue("username");

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
