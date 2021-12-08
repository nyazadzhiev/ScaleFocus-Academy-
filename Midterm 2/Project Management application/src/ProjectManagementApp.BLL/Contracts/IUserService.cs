using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Services
{
    public interface IUserService
    {
        public Task<bool> CreateUser(string username, string password, string firstName, string lastName, bool isAdmin);

        public Task<User> GetUser(string username, string password);

        public Task<List<User>> GetAllUsers();

        public Task<User> GetUser(string username);

        public Task<User> GetUser(int id);

        public Task<bool> DeleteUser(int id);

        public Task<bool> EditUser(int id, string newUsername, string newPassword, string newFirstName, string newLastName);
    }
}
