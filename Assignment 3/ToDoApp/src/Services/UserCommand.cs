using System;
using System.Collections.Generic;
using System.Text;
using ToDoAppData;
using ToDoAppEntities;
using System.Threading.Tasks;

namespace ToDoAppServices
{
    public class UserCommand
    {
        private UserService _userService;
        private UserInput userInput;
        private Validations validations = new Validations();

        public UserCommand(UserService userService)
        {
            _userService = userService;
            userInput = new UserInput();
        }

        public async Task PromptLogIn()
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
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
        }

        public void PromptLogOut()
        {
            _userService.LogOut();
        }

        public async Task PromptCreateUser()
        {
            try
            {
                string name = userInput.EnterValue("username");

                string password = userInput.EnterValue("password");

                string firstName = userInput.EnterValue("First Name");

                string lastName = userInput.EnterValue("Last Name");

                bool isAdmin = userInput.EnterRole();

                bool isSuccess = await _userService.CreateUser(name, password, firstName, lastName, isAdmin);
                if (isSuccess)
                {
                    Console.WriteLine($"User with name '{name}' added");
                }
                else
                {
                    Console.WriteLine($"User with name '{name}' already exists");
                }
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
        }

        public async Task PromptEditUser()
        {
            try
            {
                string username = userInput.EnterValue("username");
                await _userService.EditUser(username);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Username is taken");
            }
        }

        public async Task PromptDeleteUser()
        {
            try
            {
                string username = userInput.EnterValue("username");

                await _userService.DeleteUser(username);
            }
            catch (ArgumentNullException)
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
