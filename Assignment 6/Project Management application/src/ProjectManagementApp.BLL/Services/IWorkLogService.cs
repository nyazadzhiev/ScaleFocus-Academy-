using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Services
{
    public interface IWorkLogService
    {
        public Task<bool> CreateWorkLog(User user, int taskId, int workedHours);

        public Task<List<WorkLog>> GetAll(int taskId);

        public Task<WorkLog> GetWorkLog(int taskId, int workLogId);
    }
}
