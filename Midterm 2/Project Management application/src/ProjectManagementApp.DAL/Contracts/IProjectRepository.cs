using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public interface IProjectRepository
    {
        public Task AddProjectAsync(Project project);

        Task SaveChangesAsync();

        Task<Project> GetProject(string title);

        Task<Project> GetProject(int id);

        List<Project> GetAllProjects();

        List<Project> GetUserProjects(User user);

        void DeleteProject(Project project);
    }
}
