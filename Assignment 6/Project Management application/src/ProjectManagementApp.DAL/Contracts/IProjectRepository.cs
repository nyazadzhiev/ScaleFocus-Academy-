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

        public Task SaveChangesAsync();

        public Task<Project> GetProject(string title);

        public Task<Project> GetProject(int id);

        public List<Project> GetProjects(User user);

        public void DeleteProject(Project project);
    }
}
