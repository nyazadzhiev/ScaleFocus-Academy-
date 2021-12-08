using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using ProjectManagementApp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Services
{
    public class WorkLogService : IWorkLogService
    {
        private readonly IWorkLogRepository repository;
        private readonly ITaskService taskService;
        private IValidationService validations;

        public WorkLogService(IWorkLogRepository workLogRepository, ITaskService _taskService, IValidationService validation)
        {
            repository = workLogRepository;
            taskService = _taskService;
            validations = validation;
        }

        public async Task<bool> CreateWorkLog(User user, int taskId, int workedHours)
        {
            ToDoTask task = await taskService.GetTask(taskId);
            validations.EnsureTaskExist(task);
            validations.CheckTaskAccess(user, task);

            WorkLog newWorkLog = new WorkLog()
            {
                UserId = user.Id,
                ToDoTaskId = taskId,
                WorkedTime = workedHours
            };

            await repository.AddWorkLogAsync(newWorkLog);
            task.Worklogs.Add(newWorkLog);
            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteWorkLog(int taskId, int workLogId, User user)
        {
            WorkLog workLog = await GetWorkLog(taskId, workLogId, user);

            repository.DeleteWorkLog(workLog);
            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditWorkLog(int taskId, int workLogId, User user, int newWorkedTime)
        {
            WorkLog workLog = await GetWorkLog(taskId, workLogId, user);

            workLog.WorkedTime = newWorkedTime;
            await repository.SaveChangesAsync();

            return true;
        }

        public Task<bool> EditWorkLog(int taskId, int workLogId, User user)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WorkLog>> GetAll(int taskId)
        {
            ToDoTask task = await taskService.GetTask(taskId);
            validations.EnsureTaskExist(task);

            return task.Worklogs;
        }

        public async Task<WorkLog> GetWorkLog(int taskId, int workLogId, User user)
        {
            ToDoTask task = await taskService.GetTask(taskId);
            validations.EnsureTaskExist(task);
            validations.CheckTaskAccess(user, task);

            return task.Worklogs.Find(w => w.Id == workLogId);
        }
    }
}
