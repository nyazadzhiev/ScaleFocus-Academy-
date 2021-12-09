using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Contracts
{
    public interface IWorkLogService
    {
        public Task<bool> CreateWorkLog(User user, int taskId, int workedHours);

        public Task<List<WorkLog>> GetAll(int taskId);

        public Task<WorkLog> GetWorkLog(int taskId, int workLogId, User user);

        public Task<bool> EditWorkLog(int taskId, int workLogId, User user);

        public Task<bool> DeleteWorkLog(int taskId, int workLogId, User user);
    }
}
