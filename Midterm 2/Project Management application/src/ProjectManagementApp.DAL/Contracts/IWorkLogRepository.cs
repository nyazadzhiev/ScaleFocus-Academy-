using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public interface IWorkLogRepository
    {
        Task AddWorkLogAsync(WorkLog workLog);

        void DeleteWorkLog(WorkLog workLog);

        Task SaveChangesAsync();
    }
}
