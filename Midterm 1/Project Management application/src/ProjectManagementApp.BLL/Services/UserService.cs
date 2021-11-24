using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using ProjectManagementApp.BLL.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.BLL.Exceptions;
using Common;

namespace ProjectManagementApp.BLL.Services
{
    public class UserService
    {
        private readonly DatabaseContext database;
        private Validation validations;

        public UserService(DatabaseContext _database, Validation validation)
        {
            database = _database;
            validations = validation;
        }

        public async Task<bool> CreateUser(string username, string password, string firstName, string lastName, bool isAdmin, User currentUser)
        {
            validations.CheckUsername(username);

            User newUser = new User()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                IsAdmin = isAdmin
            };

            await database.Users.AddAsync(newUser);
            await database.SaveChangesAsync();

            return newUser.Id != 0;
        }

        public async Task<User> GetUser(string username, string password)
        {
            return await database.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await database.Users.ToListAsync();
        }

        public async Task<User> GetUser(string username)
        {
            return await database.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUser(int id)
        {
            return await database.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> DeleteUser(int id)
        {
            User user = await GetUser(id);
            validations.EnsureUserExist(user);

            database.Users.Remove(user);
            await database.SaveChangesAsync();

            return true;

        }

        public async Task<bool> EditUser(int id, string newUsername, string newPassword, string newFirstName, string newLastName)
        {
            User user = await GetUser(id);
            validations.EnsureUserExist(user);

            validations.CheckUsername(newUsername);

            user.Username = newUsername;
            user.Password = newPassword;
            user.FirstName = newFirstName;
            user.LastName = newLastName;


            await database.SaveChangesAsync();

            return true;
        }
    }
}
