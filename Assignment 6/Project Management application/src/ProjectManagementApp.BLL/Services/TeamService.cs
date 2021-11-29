using Common;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.BLL.Exceptions;
using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using ProjectManagementApp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository repository;
        private readonly IUserService userService;
        private IValidationService validations;

        public TeamService(ITeamRepository teamRepository, IUserService _userService, IValidationService validation)
        {
            repository = teamRepository;
            userService = _userService;
            validations = validation;
        }

        public async Task<bool> CreateTeam(string name)
        {
            validations.CheckTeamName(name);

            Team newTeam = new Team()
            {
                Name = name
            };

            await repository.AddAsync(newTeam);
            await repository.SaveChangesAsync();

            return newTeam.Id != 0;
        }

        public async Task<List<Team>> GetAll()
        {
            return await repository.GetTeamsAsync();
        }

        public async Task<Team> GetTeam(string name)
        {
            return await repository.GetTeamAsync(name);
        }

        public async Task<Team> GetTeam(int id)
        {
            return await repository.GetTeamAsync(id);
        }

        public async Task<bool> DeleteTeam(int id)
        {
            Team team= await GetTeam(id);
            validations.EnsureTeamExist(team);

            repository.DeleteTeam(team);
            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTeam(int id, string newTeamName)
        {
            Team team = await GetTeam(id);
            validations.EnsureTeamExist(team);
            validations.CheckTeamName(newTeamName);

            team.Name = newTeamName;

            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddUser(int teamId, int userId)
        {
            User userToAdd = await userService.GetUser(userId);
            validations.EnsureUserExist(userToAdd);

            Team teamFromDB = await GetTeam(teamId);
            validations.EnsureTeamExist(teamFromDB);
            validations.CanAddToTeam(teamFromDB, userToAdd);

            teamFromDB.Users.Add(userToAdd);
            userToAdd.Teams.Add(teamFromDB);
            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveUser(int teamId, int userId)
        {
            User userToRemove = await userService.GetUser(userId);
            validations.EnsureUserExist(userToRemove);

            Team teamFromDB = await GetTeam(teamId);
            validations.EnsureTeamExist(teamFromDB);

            teamFromDB.Users.Remove(userToRemove);
            userToRemove.Teams.Remove(teamFromDB);
            await repository.SaveChangesAsync();

            return true;
        }
    }
}
