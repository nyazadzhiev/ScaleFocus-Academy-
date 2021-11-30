using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Services
{
    public interface ITeamService
    {
        public Task<bool> CreateTeam(string name);

        public Task<List<Team>> GetAll();

        public Task<Team> GetTeam(string name);

        public Task<Team> GetTeam(int id);

        public Task<bool> DeleteTeam(int id);

        public Task<bool> EditTeam(int id, string newTeamName);

        public Task<bool> AddUser(int teamId, int userId);

        public Task<bool> RemoveUser(int teamId, int userId);
    }
}
