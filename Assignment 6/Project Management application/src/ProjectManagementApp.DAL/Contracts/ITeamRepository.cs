using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public interface ITeamRepository
    {
        public Task AddAsync(Team team);

        public Task SaveChangesAsync();

        public Task<List<Team>> GetTeamsAsync();

        public Task<Team> GetTeamAsync(string name);

        public Task<Team> GetTeamAsync(int id);

        public void DeleteTeam(Team team);
    }
}
