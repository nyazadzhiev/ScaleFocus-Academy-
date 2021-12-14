using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Contracts
{
    public interface ITeamService
    {
        Task<bool> CreateTeam(string name);

        Task<List<Team>> GetAll();

        Task<Team> GetTeam(string name);

        Task<Team> GetTeam(int id);

        Task<bool> DeleteTeam(int id);

        Task<bool> EditTeam(int id, string newTeamName);

        Task<bool> AddUser(int teamId, string userId);

        Task<bool> RemoveUser(int teamId, string userId);
    }
}
