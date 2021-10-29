using System;
using System.Collections.Generic;
using System.Linq;
using ToDoAppEntities;
using static ToDoAppData.FileStorage;

namespace ToDoAppServices
{
    public class UserService
    {
        private const string StoreFileName = "Users.json";
        private readonly FileDatabase _storage;
        private readonly List<User> _applicationUsers = new List<User>();
        private static int userIdGenerator = 0;

        public UserService()
        {
            _storage = new FileDatabase();
            List<User> usersFromFile = _storage.Read<List<User>>(StoreFileName);
            if (usersFromFile == null)
            {
                CreateAdmin();
            }
            else
            {
                _applicationUsers = usersFromFile;
            }
        }

        public static User CurrentUser { get; private set; }

        private void SaveToFile()
        {
            _storage.Write(StoreFileName, _applicationUsers);
        }

        public bool CreateAdmin()
        {
            userIdGenerator++;

            DateTime now = DateTime.Now;

            _applicationUsers.Add(new User()
            {
                Username = "admin",
                Password = "adminpassword",
                FirstName = "Admin",
                LastName = "Admin",
                IsAdmin = true,
                Id = userIdGenerator,
                CreatedAt = now,
                Creator = new User() { FirstName = "Admin", LastName = "Admin"}
            });

            SaveToFile();

            return true;
        }

        public bool CreateUser(string username, string password, string firstName, string lastName, bool isAdmin)
        {
            if (_applicationUsers.Any(u => u.Username == username))
            {
                return false;
            }

            userIdGenerator++;

            DateTime now = DateTime.Now;

            _applicationUsers.Add(new User()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                IsAdmin = isAdmin,
                Id = userIdGenerator,
                CreatedAt = now,
                LastEdited = null,
                Creator = CurrentUser
            });

            SaveToFile();

            return true;
        }

        public void Login(string userName, string password)
        {
            CurrentUser = _applicationUsers.FirstOrDefault(u => u.Username == userName && u.Password == password);
        }

        public void LogOut()
        {
            CurrentUser = null;
        }

        public List<User> GetAllUsers()
        {
            return _applicationUsers;
        }

        public User GetUser(int id)
        {
            return _applicationUsers.FirstOrDefault(u => u.Id == id);
        }

        public User GetUser(string username)
        {
            return _applicationUsers.FirstOrDefault(u => u.Username == username);
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
                string newValues = Console.ReadLine();
                user.Username = newValues;

                Console.WriteLine("Enter new password");
                newValues = Console.ReadLine();
                user.Password = newValues;

                Console.WriteLine("Enter new First Name");
                newValues = Console.ReadLine();
                user.FirstName = newValues;

                Console.WriteLine("Enter new Last Name");
                newValues = Console.ReadLine();
                user.LastName = newValues;

                user.LastEdited = DateTime.Now;

                Console.WriteLine("You successfully edited the user");

                SaveToFile();

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
                _applicationUsers.Remove(user);
                Console.WriteLine($"You Deleted user {username}");
                SaveToFile();

                return true;
            }
        }
    }
}
