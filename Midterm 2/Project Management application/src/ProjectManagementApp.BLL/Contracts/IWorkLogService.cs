using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Contracts
{
    public interface IWorkLogService
    {
        Task<bool> CreateWorkLog(User user, int taskId, int workedHours);

        Task<List<WorkLog>> GetAll(int taskId);

        Task<WorkLog> GetWorkLog(int taskId, int workLogId, User user);

        Task<bool> EditWorkLog(int taskId, int workLogId, User user, int newWorkedTime);

        Task<bool> DeleteWorkLog(int taskId, int workLogId, User user);
    }
}
