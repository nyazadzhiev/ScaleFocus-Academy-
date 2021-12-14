using Common;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.BLL.Contracts;
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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository repository;
        private readonly ITeamService teamService;
        private IValidationService validations;

        public ProjectService(IProjectRepository projectRepository, ITeamService _teamService, IValidationService validation)
        {
            repository = projectRepository;
            teamService = _teamService;
            validations = validation;
        }

        public async Task<bool> CreateProject(string title, User currentUser)
        {
            validations.CheckProjectName(title);

            Project newProject = new Project()
            {
                Title = title,
                OwnerId = currentUser.Id
            };

            await repository.AddProjectAsync(newProject);
            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<Project> GetProject(string title, User user)
        {
            return await repository.GetProject(title);
        }

        public async Task<Project> GetProject(int id, User user)
        {
            return await repository.GetProject(id);
        }

        public List<Project> GetAllProjects()
        {
            return repository.GetAllProjects();
        }

        public List<Project> GetMyProjects(User user)
        {
            return repository.GetUserProjects(user);
        }

        public async Task<bool> DeleteProject(int id, User user)
        {
            Project project = await GetProject(id, user);
            validations.EnsureProjectExist(project);

            repository.DeleteProject(project);
            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditProject(int id, string newProjectTitle, User user)
        {
            Project project = await GetProject(id, user);
            validations.EnsureProjectExist(project);
            validations.CheckProjectName(newProjectTitle);

            project.Title = newProjectTitle;

            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddTeam(int projectId, int teamId, User user)
        {
            Project project = await GetProject(projectId, user);
            validations.EnsureProjectExist(project);

            Team team = await teamService.GetTeam(teamId);
            validations.EnsureTeamExist(team);
            validations.CanAddToProject(project, team);

            project.Teams.Add(team);
            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveTeam(int projectId, int teamId, User user)
        {
            Project project = await GetProject(projectId, user);
            validations.EnsureProjectExist(project);

            Team team = await teamService.GetTeam(teamId);
            validations.EnsureTeamExist(team);

            project.Teams.Remove(team);
            await repository.SaveChangesAsync();

            return true;
        }
    }
}
