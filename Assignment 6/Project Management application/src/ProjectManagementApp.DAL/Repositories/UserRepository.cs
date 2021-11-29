using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext database;

        public UserRepository(DatabaseContext context)
        {
            database = context;
        }

        public async Task AddUser(User user)
        {
            await database.Users.AddAsync(user);
        }

        public async Task<User> GetUserAsync(string username, string password)
        {
           return await database.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await database.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await database.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await database.Users.ToListAsync();
        }

        public void DeleteUser(User user)
        {
            database.Users.Remove(user);
        }

        public async Task SaveChangesAsync()
        {
            await database.SaveChangesAsync();
        }
    }
}
