using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DatabaseContext database;

        public ProjectRepository(DatabaseContext context)
        {
            database = context;
        }

        public async Task AddProjectAsync(Project project)
        {
            await database.AddAsync(project);
        } 

        public async Task SaveChangesAsync()
        {
            await database.SaveChangesAsync();
        }

        public async Task<Project> GetProject(string title)
        {
            return await database.Projects.FirstOrDefaultAsync(p => p.Title == title);
        }

        public async Task<Project> GetProject(int id)
        {
            return await database.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }

        public List<Project> GetProjects(User user)
        {
            return database.Projects.Where(p => p.OwnerId == user.Id).ToList();
        }

        public void DeleteProject(Project project)
        {
            database.Projects.Remove(project);
        }
    }
}
