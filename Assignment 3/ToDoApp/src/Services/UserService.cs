using System;
using System.Collections.Generic;
using System.Linq;
using ToDoAppData;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class UserService
    {
        private readonly DatabaseContext _database;
        private UserInput userInput;
        private Validations validations;

        public UserService(DatabaseContext database)
        {
            _database = database;
            userInput = new UserInput();
            validations = new Validations();
        }

        public static User CurrentUser { get; private set; }

        public bool CreateUser(string username, string password, string firstName, string lastName, bool isAdmin)
        {
            if (_database.Users.Any(u => u.Username == username))
            {
                return false;
            }

            DateTime now = DateTime.Now;

            User newUser = new User()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                IsAdmin = isAdmin,
                CreatedAt = now,
                LastEdited = now,
                CreatorId = UserService.CurrentUser.Id,
                ModifierId = UserService.CurrentUser.Id
            };

            _database.Users.Add(newUser);

            _database.SaveChanges();

            return newUser.Id != 0;
        }

        public void Login(string userName, string password)
        {
            CurrentUser = _database.Users.FirstOrDefault(u => u.Username == userName && u.Password == password);
        }

        public void LogOut()
        {
            CurrentUser = null;
        }

        public List<User> GetAllUsers()
        {
            return _database.Users.ToList();
        }

        public User GetUser(int id)
        {
            return _database.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUser(string username)
        {
            return _database.Users.FirstOrDefault(u => u.Username == username);
        }

        public bool EditUser(string username)
        {
            User user = GetUser(username);

            bool isValidUser = validations.EnsureUserExist(user);

            if (!isValidUser)
            {
                return false;
            }
            else
            {
                string newUsername = userInput.EnterValue("new username");

                string newPassword = userInput.EnterValue("new password");

                string newFirstName = userInput.EnterValue("new First Name");
 
                string newLastName  = userInput.EnterValue("new Last Name");

                DateTime dateTime = DateTime.Now;

                Console.WriteLine("You successfully edited the user");

                user.Username = newUsername;
                user.Password = newPassword;
                user.FirstName = newFirstName;
                user.LastName = newLastName;
                user.LastEdited = DateTime.Now;
                user.ModifierId = UserService.CurrentUser.Id;

                _database.SaveChanges();

                return true;
            }
        }

        public bool DeleteUser(string username)
        {
            User user = GetUser(username);

            bool isValidUser = validations.EnsureUserExist(user);

            if (!isValidUser)
            {
                return false;
            }
            else
            {
                _database.Users.Remove(user);
                _database.SaveChanges();
                Console.WriteLine($"You Deleted user {username}");

                return true;
            }
        }
    }
}

