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
    public class ProjectService
    {
        private readonly DatabaseContext database;
        private readonly UserService userService;
        private readonly TeamService teamService;
        private Validation validations;

        public ProjectService(DatabaseContext _database, UserService _userService, TeamService _teamService, Validation validation)
        {
            database = _database;
            userService = _userService;
            teamService = _teamService;
            validations = validation;
        }

        public async Task<bool> CreateProject(string title, User currentUser)
        {
            if (database.Projects.Any(p => p.Title == title))
            {
                return false;
            }

            Project newProject = new Project()
            {
                Title = title,
                OwnerId = currentUser.Id
            };

            await database.Projects.AddAsync(newProject);
            await database.SaveChangesAsync();

            return newProject.Id != 0;
        }

        public async Task<Project> GetProject(string title, User user)
        {
            return await database.Projects.FirstOrDefaultAsync(p => p.Title == title && p.OwnerId == user.Id);
        }

        public async Task<Project> GetProject(int id, User user)
        {
            return await database.Projects.FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == user.Id);
        }

        public async Task<List<Project>> GetAll(User user)
        {
            return await database.Projects.Where(p => p.OwnerId == user.Id).ToListAsync();
        }

        public async Task<bool> DeleteProject(string title, User user)
        {
            Project project = await GetProject(title, user);

            validations.EnsureProjectExist(project);

            database.Projects.Remove(project);
            await database.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditProject(int id, string newProjectTitle, User user)
        {
            Project project = await GetProject(id, user);

            validations.EnsureProjectExist(project);

            bool isValid = validations.CheckProjectName(newProjectTitle);

            if (isValid)
            {
                throw new ProjectExistException(String.Format(Constants.Exist, "Project"));
            }

            project.Title = newProjectTitle;

            await database.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddTeam(int projectId, int teamId, User user)
        {
            Project project = await GetProject(projectId, user);
            validations.EnsureProjectExist(project);

            Team team = await teamService.GetTeam(teamId);
            validations.EnsureTeamExist(team);

            bool teamExistInProject = validations.CanAddToProject(project, team);

            if (teamExistInProject)
            {
                throw new ProjectExistException(String.Format(Constants.Exist, "Project"));
            }

            project.Teams.Add(team);
            await database.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveTeam(int projectId, int teamId, User user)
        {
            Project project = await GetProject(projectId, user);
            validations.EnsureProjectExist(project);

            Team team = await teamService.GetTeam(teamId);
            validations.EnsureTeamExist(team);

            project.Teams.Remove(team);
            await database.SaveChangesAsync();

            return true;
        }
    }
}
