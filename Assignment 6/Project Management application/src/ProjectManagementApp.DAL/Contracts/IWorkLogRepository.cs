using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public interface IWorkLogRepository
    {
        public Task AddWorkLogAsync(WorkLog workLog);

        public Task SaveChangesAsync();
    }
}
