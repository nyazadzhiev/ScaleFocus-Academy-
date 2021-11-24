using Common;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.BLL.Exceptions;
using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Services
{
    public class TeamService
    {
        private readonly DatabaseContext database;
        private readonly UserService userService;
        private Validation validations;

        public TeamService(DatabaseContext _database, UserService _userService, Validation validation)
        {
            database = _database;
            userService = _userService;
            validations = validation;
        }

        public async Task<bool> CreateTeam(string name)
        {
            if(database.Teams.Any(t => t.Name == name))
            {
                return false;
            }

            Team newTeam = new Team()
            {
                Name = name
            };

            await database.Teams.AddAsync(newTeam);
            await database.SaveChangesAsync();

            return newTeam.Id != 0;
        }

        public async Task<List<Team>> GetAll()
        {
            return await database.Teams.ToListAsync();
        }

        public async Task<Team> GetTeam(string name)
        {
            return await database.Teams.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<Team> GetTeam(int id)
        {
            return await database.Teams.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> DeleteTeam(string name)
        {
            Team team = await GetTeam(name);

            validations.EnsureTeamExist(team);

            database.Teams.Remove(team);
            await database.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTeam(int id, string newTeamName)
        {
            Team team = await GetTeam(id);

            validations.EnsureTeamExist(team);

            bool isValid = validations.CheckTeamName(newTeamName);

            if (isValid)
            {
                throw new TeamExistException(String.Format(Constants.Exist, "Team"));
            }

            team.Name = newTeamName;

            await database.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddUser(int teamId, int userId)
        {
            User userToAdd = await userService.GetUser(userId);
            validations.EnsureUserExist(userToAdd);

            Team teamFromDB = await GetTeam(teamId);
            validations.EnsureTeamExist(teamFromDB);

            bool UserExistInTeam = validations.CanAddToTeam(teamFromDB, userToAdd);

            if (UserExistInTeam)
            {
                throw new UserExistException(String.Format(Constants.Exist, "User"));
            }

            teamFromDB.Users.Add(userToAdd);
            await database.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveUser(int teamId, int userId)
        {
            User userToRemove = await userService.GetUser(userId);
            validations.EnsureUserExist(userToRemove);

            Team teamFromDB = await GetTeam(teamId);
            validations.EnsureTeamExist(teamFromDB);

            teamFromDB.Users.Remove(userToRemove);
            await database.SaveChangesAsync();

            return true;
        }
    }
}
