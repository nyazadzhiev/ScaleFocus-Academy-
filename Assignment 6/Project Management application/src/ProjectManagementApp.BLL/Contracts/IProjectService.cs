using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Services
{
    public interface IProjectService
    {
        public Task<bool> CreateProject(string title, User currentUser);

        public Task<Project> GetProject(string title, User user);

        public Task<Project> GetProject(int id, User user);

        public List<Project> GetAll(User user);

        public Task<bool> DeleteProject(int id, User user);

        public Task<bool> EditProject(int id, string newProjectTitle, User user);

        public Task<bool> AddTeam(int projectId, int teamId, User user);

        public Task<bool> RemoveTeam(int projectId, int teamId, User user);
    }
}
