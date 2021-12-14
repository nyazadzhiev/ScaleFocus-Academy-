using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Contracts
{
    public interface IProjectService
    {
        Task<bool> CreateProject(string title, User currentUser);

        Task<Project> GetProject(string title, User user);

        Task<Project> GetProject(int id, User user);

        List<Project> GetMyProjects(User user);

        List<Project> GetAllProjects();

        Task<bool> DeleteProject(int id, User user);

        Task<bool> EditProject(int id, string newProjectTitle, User user);

        Task<bool> AddTeam(int projectId, int teamId, User user);

        Task<bool> RemoveTeam(int projectId, int teamId, User user);
    }
}
