using Common;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.BLL.Contracts;
using ProjectManagementApp.BLL.Exceptions;
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
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository repository;
        private readonly IProjectService projectService;
        private IValidationService validations;

        public TaskService(ITaskRepository taskRepository, IProjectService _projectService, IValidationService validation)
        {
            repository = taskRepository;
            projectService = _projectService;
            validations = validation;
        }

        public async Task<bool> CreateTask(string title, string description, bool isCompleted, int projectId,  User currentUser, string userId)
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

            await repository.AddTaskAsync(newTask);
            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<ToDoTask> GetTask(int id)
        {
            return await repository.GetTaskAsync(id);
        }

        public async Task<ToDoTask> GetTask(string title)
        {
            return await repository.GetTaskAsync(title);
        }

        public async Task<ToDoTask> GetTask(int taskId, int projectId, User user)
        {
            Project projectFromDB = await projectService.GetProject(projectId, user);
            validations.EnsureProjectExist(projectFromDB);
            validations.CheckProjectAccess(user, projectFromDB);

            ToDoTask task = await repository.GetTaskAsync(taskId);
            validations.EnsureTaskExist(task);
            validations.CheckTaskAccess(user, task);

            return task;
        }

        public async Task<List<ToDoTask>> GetAll(int projectId, User currentUser)
        {
            Project project = await projectService.GetProject(projectId, currentUser);
            validations.EnsureProjectExist(project);
            validations.CheckProjectAccess(currentUser, project);

            return await repository.GetTasksAsync(projectId);
        }

        public async Task<bool> DeleteTask(int taskId, int projectId, User user)
        {
            Project project = await projectService.GetProject(projectId, user);
            validations.EnsureProjectExist(project);
            validations.CheckProjectAccess(user, project);

            ToDoTask task = await GetTask(taskId, projectId, user);
            validations.EnsureProjectExist(project);

            repository.DeleteTask(task);
            await repository.SaveChangesAsync();

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

            await repository.SaveChangesAsync();

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

            await repository.SaveChangesAsync();

            return true;
        }
    }
}
