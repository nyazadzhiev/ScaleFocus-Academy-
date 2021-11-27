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

        public async Task<bool> CreateTask(string title, string description, bool isCompleted, int projectId,  User currentUser, int userId)
        {
            validations.CheckTaskName(title);

            Project project = await projectService.GetProject(projectId, currentUser);
            validations.EnsureProjectExist(project);
            validations.CheckProjectAccess(currentUser, project);

            ToDoTask newTask = new ToDoTask()
            {
                Title = title,
                Description = description,
                IsCompleted = isCompleted,
                ProjectId = projectId,
                AsigneeId = userId,
                OwnerId = currentUser.Id
            };

            await database.ToDoTasks.AddAsync(newTask);
            await database.SaveChangesAsync();

            return newTask.Id != 0;
        }

        public async Task<ToDoTask> GetTask(int id)
        {
            return await database.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ToDoTask> GetTask(string title)
        {
            return await database.ToDoTasks.FirstOrDefaultAsync(t => t.Title == title);
        }

        public async Task<ToDoTask> GetTask(int taskId, int projectId, User user)
        {
            Project projectFromDB = await projectService.GetProject(projectId, user);
            validations.EnsureProjectExist(projectFromDB);
            validations.CheckProjectAccess(user, projectFromDB);

            return await database.ToDoTasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<List<ToDoTask>> GetAll(int projectId, User currentUser)
        {
            Project project = await projectService.GetProject(projectId, currentUser);
            validations.EnsureProjectExist(project);
            validations.CheckProjectAccess(currentUser, project);

            return await database.ToDoTasks.Where(t => t.ProjectId == projectId).ToListAsync();
        }

        public async Task<bool> DeleteTask(int taskId, int projectId, User user)
        {
            Project project = await projectService.GetProject(projectId, user);
            validations.EnsureProjectExist(project);
            validations.CheckProjectAccess(user, project);

            ToDoTask task = await GetTask(taskId, projectId, user);
            validations.EnsureProjectExist(project);

            database.ToDoTasks.Remove(task);
            await database.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTask(int taskId,int projectId, User user, string newTaskTitle, string newDesc, bool newStatus)
        {
            Project project = await projectService.GetProject(projectId, user);
            validations.EnsureProjectExist(project);
            validations.CheckProjectAccess(user, project);

            ToDoTask task = await GetTask(taskId, projectId, user);
            validations.EnsureTaskExist(task);
            validations.CheckTaskName(newTaskTitle);

            task.Title = newTaskTitle;
            task.Description = newDesc;
            task.IsCompleted = newStatus;

            await database.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeStatus(int taskId, int projectId, User user)
        {
            Project project = await projectService.GetProject(projectId, user);
            validations.EnsureProjectExist(project);
            validations.CheckProjectAccess(user, project);

            ToDoTask task = await GetTask(taskId, projectId, user);
            validations.EnsureTaskExist(task);

            task.IsCompleted = !task.IsCompleted;

            await database.SaveChangesAsync();

            return true;
        }
    }
}
