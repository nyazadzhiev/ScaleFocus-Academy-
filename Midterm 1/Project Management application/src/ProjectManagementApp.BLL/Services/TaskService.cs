using Common;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.BLL.Exceptions;
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
    public class TaskService
    {
        private readonly DatabaseContext database;
        private readonly TeamService teamService;
        private readonly ProjectService projectService;
        private Validation validations;

        public TaskService(DatabaseContext _database, TeamService _teamService, ProjectService _projectService, Validation validation)
        {
            database = _database;
            teamService = _teamService;
            projectService = _projectService;
            validations = validation;
        }

        public async Task<bool> CreateTask(string title, string description, bool isCompleted, Project project,  User owner, User asignee)
        {
            if (database.ToDoTasks.Any(t => t.Title == title))
            {
                return false;
            }

            ToDoTask newTask = new ToDoTask()
            {
                Title = title,
                Description = description,
                IsCompleted = isCompleted,
                ProjectId = project.Id,
                AsigneeId = asignee.Id,
                OwnerId = owner.Id
            };

            await database.ToDoTasks.AddAsync(newTask);
            await database.SaveChangesAsync();

            return newTask.Id != 0;
        }

        public async Task<ToDoTask> GetTask(string title)
        {
            return await database.ToDoTasks.FirstOrDefaultAsync(t => t.Title == title);
        }

        public async Task<ToDoTask> GetTask(int id)
        {
            return await database.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<ToDoTask>> GetAll(Project project)
        {
            return await database.ToDoTasks.Where(t => t.ProjectId == project.Id).ToListAsync();
        }

        public async Task<bool> DeleteTask(string title)
        {
            ToDoTask task = await GetTask(title);

            validations.EnsureTaskExist(task);

            database.ToDoTasks.Remove(task);
            await database.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTask(int id, string newTaskTitle, string newDesc, bool newStatus)
        {
            ToDoTask task = await GetTask(id);
            validations.EnsureTaskExist(task);

            bool isValid = validations.CheckTaskName(newTaskTitle);

            if (isValid)
            {
                throw new TaskExistException(String.Format(Constants.Exist, "Task"));
            }

            task.Title = newTaskTitle;
            task.Description = newDesc;
            task.IsCompleted = newStatus;

            await database.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeStatus(int id)
        {
            ToDoTask task = await GetTask(id);
            validations.EnsureTaskExist(task);

            task.IsCompleted = !task.IsCompleted;

            await database.SaveChangesAsync();

            return true;
        }
    }
}
