using System;
using System.Collections.Generic;
using System.Linq;
using ToDoAppData;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class UserService
    {
        private readonly UserRepository _database;
        private static int userIdGenerator = 0;

        public UserService(UserRepository database)
        {
            _database = database;
            List<User> usersFromDB = database.GetUsers();
            if(usersFromDB.Count == 0)
            {
                CreateAdmin();
            }
        }

        public static User CurrentUser { get; private set; }

        public bool CreateAdmin()
        {
            userIdGenerator++;

            DateTime now = DateTime.Now;

            return _database.CreateUser(new User()
            {
                Username = "admin",
                Password = "adminpassword",
                FirstName = "Admin",
                LastName = "Admin",
                IsAdmin = true,
                Id = userIdGenerator,
                CreatedAt = now,
                LastEdited = now,
                ModifierId = userIdGenerator,
                CreatorId = userIdGenerator
            });
        }

        public bool CreateUser(string username, string password, string firstName, string lastName, bool isAdmin)
        {
            if (_database.GetUser(username) != null)
            {
                return false;
            }

            userIdGenerator++;

            DateTime now = DateTime.Now;

            return _database.CreateUser(new User()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                IsAdmin = isAdmin,
                Id = userIdGenerator,
                CreatedAt = now,
                LastEdited = now,
                CreatorId = UserService.CurrentUser.Id,
                ModifierId = UserService.CurrentUser.Id
            }); 
        }

        public void Login(string userName, string password)
        {
            CurrentUser = _database.GetUsers().FirstOrDefault(u => u.Username == userName && u.Password == password);
        }

        public void LogOut()
        {
            CurrentUser = null;
        }

        public List<User> GetAllUsers()
        {
            return _database.GetUsers();
        }

        public User GetUser(int id)
        {
            return _database.GetUser(id);
        }

        public User GetUser(string username)
        {
            return _database.GetUser(username);
        }

        public bool EditUser(string username)
        {
            User user = GetUser(username);

            if (user == null)
            {
                Console.WriteLine($"There isn't user with username {username}");

                return false;
            }
            else
            {
                Console.WriteLine("Enter new username");
                string newUsername = Console.ReadLine();

                Console.WriteLine("Enter new password");
                string newPassword = Console.ReadLine();

                Console.WriteLine("Enter new First Name");
                string newFirstName = Console.ReadLine();
 
                Console.WriteLine("Enter new Last Name");
                string newLastName  = Console.ReadLine();

                DateTime dateTime = DateTime.Now;

                Console.WriteLine("You successfully edited the user");

                _database.EditUser(username, newUsername, newPassword, newFirstName, newLastName, dateTime);

                return true;
            }
        }

        public bool DeleteUser(string username)
        {
            User user = GetUser(username);

            if (user == null)
            {
                Console.WriteLine($"There isn't user with username {username}");

                return false;
            }
            else
            {
                _database.DeleteUser(username);
                Console.WriteLine($"You Deleted user {username}");

                return true;
            }
        }
    }
}

