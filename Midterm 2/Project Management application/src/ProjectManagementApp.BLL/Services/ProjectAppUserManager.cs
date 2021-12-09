using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProjectManagementApp.BLL.Contracts;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Services
{
    public class ProjectAppUserManager : UserManager<User>, IUserManager
    {
        public ProjectAppUserManager(IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger) :
            base(store,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger)
        {

        }

        public async Task CreateUserAsync(User user, string password)
        {
            await CreateAsync(user, password);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            User user = await FindByIdAsync(id);

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string name)
        {
            User user = await FindByNameAsync(name);

            return user;
        }

        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            return (await GetRolesAsync(user)).ToList();
        }

        public async Task UpdateUserAsync(User user, string newUsername, string newPassword, string newFirstName, string newLastName)
        {
            user.UserName = newUsername;
            user.FirstName = newFirstName;
            user.LastName = newLastName;
            await UpdatePasswordHash(user, newPassword, true);
            await UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(User user)
        {
            await DeleteAsync(user);
        }

        public async Task<bool> IsUserInRole(string userId, string roleId)
        {
            User user = await FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            return await IsInRoleAsync(user, roleId);
        }

        public async Task<bool> ValidateUserCredentials(string userName, string password)
        {
            User user = await FindByNameAsync(userName);
            if (user != null)
            {
                bool result = await CheckPasswordAsync(user, password);
                return result;
            }
            return false;
        }
    }
}
