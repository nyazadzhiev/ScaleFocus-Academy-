using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Services
{
    public class WorkLogService
    {
        private readonly DatabaseContext database;
        private readonly UserService userService;
        private readonly TaskService taskService;
        private Validation validations;

        public WorkLogService(DatabaseContext _database, UserService _userService, TaskService _taskService, Validation validation)
        {
            database = _database;
            userService = _userService;
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

            await database.WorkLogs.AddAsync(newWorkLog);
            task.Worklogs.Add(newWorkLog);
            await database.SaveChangesAsync();

            return newWorkLog.Id != 0;
        }

        public async Task<List<WorkLog>> GetAll(int taskId)
        {
            ToDoTask task = await taskService.GetTask(taskId);
            validations.EnsureTaskExist(task);

            return task.Worklogs;
        }

        public async Task<WorkLog> GetWorkLog(int taskId, int workLogId)
        {
            ToDoTask task = await taskService.GetTask(taskId);
            validations.EnsureTaskExist(task);

            return task.Worklogs.Find(w => w.Id == workLogId);
        }
    }
}
