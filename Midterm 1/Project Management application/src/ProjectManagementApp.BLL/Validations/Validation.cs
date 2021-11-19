using ProjectManagementApp.BLL.Exceptions;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManagementApp.BLL.Validations
{
    public class Validation
    {
        private readonly DatabaseContext database;

        public Validation(DatabaseContext _database)
        {
            database = _database;
        }

        public bool EnsureUserExist(User user)
        {
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            else
            {
                return true;
            }
        }

        public bool CheckRole(User user)
        {
            if (!user.IsAdmin)
            {
                throw new UnauthorizedUserException();
            }
            else
            {
                return true;
            }
        }

        public bool CheckUsername(string username)
        {
            return database.Users.Any(u => u.Username == username);
        }
    }
}
