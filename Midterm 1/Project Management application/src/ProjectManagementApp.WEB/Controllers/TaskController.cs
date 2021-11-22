using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagementApp.BLL.Services;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Models.Requests;
using ProjectManagementApp.DAL.Entities;
using ProjectManagementApp.WEB.Auth;
using Common;
using ProjectManagementApp.DAL.Models.Responses;
using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.BLL.Exceptions;

namespace ProjectManagementApp.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly UserService userService;
        private readonly TeamService teamService;
        private readonly TaskService taskService;
        private readonly ProjectService projectService;
        private Validation validations;

        public TaskController(DatabaseContext database) : base()
        {
            userService = new UserService(database, validations);
            teamService = new TeamService(database, userService, validations);
            projectService = new ProjectService(database, userService, teamService, validations);
            taskService = new TaskService(database, teamService, projectService, validations);
            validations = new Validation(database);
        }

        [HttpGet("/Project/{projectId}")]
        public async Task<ActionResult> GetAll(int projectId)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

            List<TaskResponseModel> tasks = new List<TaskResponseModel>();

            foreach (ToDoTask task in await taskService.GetAll(projectId, currentUser))
            {
                tasks.Add(new TaskResponseModel()
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    AsigneeId = task.AsigneeId,
                    ProjectId = task.ProjectId,
                    OwnerId = task.OwnerId
                });
            }

            return Ok(tasks);
        }

        [HttpGet("{taskId}/Project/{projectId}")]
        public async Task<ActionResult<TaskResponseModel>> Get(int taskId, int projectId)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

            Project projectFromDB = await projectService.GetProject(projectId, currentUser);
            validations.EnsureProjectExist(projectFromDB);

            ToDoTask taskFromDB = await taskService.GetTask(taskId);
            validations.EnsureTaskExist(taskFromDB);

            return new TaskResponseModel()
            {
                Id = taskFromDB.Id,
                Title = taskFromDB.Title,
                Description = taskFromDB.Description,
                IsCompleted = taskFromDB.IsCompleted,
                AsigneeId = taskFromDB.AsigneeId,
                ProjectId = taskFromDB.ProjectId
            };
        }

        [HttpPost("/Project/{projectId}/User/{userId}")]
        public async Task<ActionResult> Post(TaskRequestModel task, int projectId, int userId)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

            bool isCreated = await taskService.CreateTask(task.Title, task.Description, task.IsCompleted, projectId, currentUser, userId);

            if (isCreated && ModelState.IsValid)
            {
                ToDoTask taskFromDB = await taskService.GetTask(task.Title);

                return CreatedAtAction(nameof(Post), new { id = taskFromDB.Id }, Constants.Created);
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [HttpPut("{taskId}/Project/{projectId}")]
        public async Task<ActionResult<TaskResponseModel>> Put(TaskRequestModel task, int taskId, int projectId)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

            Project project = await projectService.GetProject(projectId, currentUser);
            validations.EnsureProjectExist(project);

            if (await taskService.EditTask(taskId, task.Title, task.Description, task.IsCompleted))
            {
                ToDoTask edited = await taskService.GetTask(task.Title);

                return new TaskResponseModel()
                {
                    Id = edited.Id,
                    Title = edited.Title,
                    Description = edited.Description,
                    IsCompleted = edited.IsCompleted,
                    OwnerId = edited.OwnerId,
                    AsigneeId = edited.AsigneeId,
                    ProjectId = edited.ProjectId
                };
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [HttpDelete("{taskId}/Project/{projectId}")]
        public async Task<ActionResult> Delete(int taskId, int projectId)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

            Project projectFromDB = await projectService.GetProject(projectId, currentUser);
            validations.EnsureProjectExist(projectFromDB);

            ToDoTask taskFromDB = await taskService.GetTask(taskId);
            validations.EnsureProjectExist(projectFromDB);

            if (await taskService.DeleteTask(taskFromDB.Title))
            {
                return Ok(Constants.Deleted);
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }
    }
}
