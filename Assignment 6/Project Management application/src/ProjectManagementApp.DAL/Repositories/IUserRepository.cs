using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public interface IUserRepository
    {
        public Task AddUser(User user);

        public Task<User> GetUserAsync(string username, string password);

        public Task<User> GetUserAsync(string username);

        public Task<User> GetUserAsync(int id);

        public Task<List<User>> GetUsersAsync();

        public void DeleteUser(User user);

        public Task SaveChangesAsync();
    }
}
