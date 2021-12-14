using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public interface ITeamRepository
    {
        Task AddAsync(Team team);

        Task SaveChangesAsync();

        Task<List<Team>> GetTeamsAsync();

        Task<Team> GetTeamAsync(string name);

        Task<Team> GetTeamAsync(int id);

        void DeleteTeam(Team team);
    }
}
