using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagementApp.BLL.Services;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using Common;
using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.BLL.Exceptions;
using System.Linq;
using ProjectManagementApp.WEB.Models.Responses;
using ProjectManagementApp.WEB.Models.Requests;
using ProjectManagementApp.BLL.Contracts;

namespace ProjectManagementApp.WEB.Controllers
{
    [Route("api/Project/{projectId}/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITeamService teamService;
        private readonly ITaskService taskService;
        private readonly IProjectService projectService;
        private IValidationService validations;

        public TaskController(IValidationService validation, IUserService _userService, ITeamService _teamService, IProjectService _projectService, ITaskService _taskService) : base()
        {
            validations = validation;
            userService = _userService;
            teamService = _teamService;
            projectService = _projectService;
            taskService = _taskService;
        }

        [Authorize(Policy = "ProjectAccess")]
        [HttpGet]
        public async Task<ActionResult> GetAll(int projectId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

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
                    OwnerId = task.OwnerId,
                    TotalWorkedHours = task.Worklogs.Sum(w => w.WorkedTime)
                });
            }

            return Ok(tasks);
        }

        [Authorize]
        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskResponseModel>> Get(int taskId, int projectId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            ToDoTask taskFromDB = await taskService.GetTask(taskId, projectId, currentUser);

            return new TaskResponseModel()
            {
                Id = taskFromDB.Id,
                Title = taskFromDB.Title,
                Description = taskFromDB.Description,
                IsCompleted = taskFromDB.IsCompleted,
                AsigneeId = taskFromDB.AsigneeId,
                ProjectId = taskFromDB.ProjectId,
                OwnerId = taskFromDB.OwnerId,
                TotalWorkedHours = taskFromDB.Worklogs.Sum(w => w.WorkedTime)
            };
        }

        [Authorize(Policy = "ProjectAccess")]
        [HttpPost("User/{userId}")]
        public async Task<ActionResult> Post(TaskRequestModel task, int projectId, string userId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            bool isCreated = await taskService.CreateTask(task.Title, task.Description, task.IsCompleted, projectId, currentUser, userId);

            if (isCreated && ModelState.IsValid)
            {
                ToDoTask taskFromDB = await taskService.GetTaskByTitle(task.Title);

                return CreatedAtAction(nameof(Post), new { id = taskFromDB.Id }, String.Format(Constants.Created, "Task"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Policy = "TaskAccess")]
        [HttpPut("{taskId}")]
        public async Task<ActionResult<TaskResponseModel>> Put(TaskRequestModel task, int taskId, int projectId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            if (await taskService.EditTask(taskId, projectId, currentUser, task.Title, task.Description, task.IsCompleted))
            {
                ToDoTask edited = await taskService.GetTaskById(taskId);

                return new TaskResponseModel()
                {
                    Id = edited.Id,
                    Title = edited.Title,
                    Description = edited.Description,
                    IsCompleted = edited.IsCompleted,
                    OwnerId = edited.OwnerId,
                    AsigneeId = edited.AsigneeId,
                    ProjectId = edited.ProjectId,
                    TotalWorkedHours = edited.Worklogs.Sum(w => w.WorkedTime)
                };
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Policy = "TaskAccess")]
        [HttpDelete("{taskId}")]
        public async Task<ActionResult> Delete(int taskId, int projectId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            if (await taskService.DeleteTask(taskId, projectId, currentUser))
            {
                return Ok(String.Format(Constants.Deleted, "Task"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Policy = "TaskAccess")]
        [HttpPut("{taskId}/ChangeStatus")]
        public async Task<ActionResult> ChangeStatus(int taskId, int projectId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            if(await taskService.ChangeStatus(taskId, projectId, currentUser))
            {
                return Ok(Constants.StatusChanged);
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Policy = "TaskAccess")]
        [HttpPut("{taskId}/User/{userId}/Reassign")]
        public async Task<ActionResult> ReassignTask(int taskId, int projectId, string userId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            if (await taskService.Reassign(taskId, projectId, userId, currentUser))
            {
                return Ok(Constants.TaskReassigned);
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }
    }
}
