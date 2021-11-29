using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DatabaseContext database;

        public TeamRepository(DatabaseContext context)
        {
            database = context;
        }

        public async Task AddAsync(Team team)
        {
            await database.Teams.AddAsync(team);
        }

        public async Task SaveChangesAsync()
        {
            await database.SaveChangesAsync();
        }

        public async Task<List<Team>> GetTeamsAsync()
        {
            return await database.Teams.ToListAsync();
        }

        public async Task<Team> GetTeamAsync(string name)
        {
            return await database.Teams.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<Team> GetTeamAsync(int id)
        {
            return await database.Teams.FirstOrDefaultAsync(t => t.Id == id);
        }

        public void DeleteTeam(Team team)
        {
            database.Teams.Remove(team);
        }
    }
}
