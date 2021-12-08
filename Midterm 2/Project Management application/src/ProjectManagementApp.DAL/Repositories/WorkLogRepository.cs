using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public class WorkLogRepository : IWorkLogRepository
    {
        private readonly DatabaseContext database;

        public WorkLogRepository(DatabaseContext context)
        {
            database = context;
        }

        public async Task AddWorkLogAsync(WorkLog workLog)
        {
            await database.WorkLogs.AddAsync(workLog);
        }

        public void DeleteWorkLog(WorkLog workLog)
        {
            database.WorkLogs.Remove(workLog);
        }

        public async Task SaveChangesAsync()
        {
            await database.SaveChangesAsync();
        }
    }
}
