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
using Common;
using ProjectManagementApp.DAL.Models.Responses;
using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.BLL.Exceptions;
using ProjectManagementApp.BLL.Contracts;

namespace ProjectManagementApp.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkLogController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITeamService teamService;
        private readonly ITaskService taskService;
        private readonly IProjectService projectService;
        private readonly IWorkLogService workLogService;
        private IValidationService validations;

        public WorkLogController(IValidationService validation, IUserService _userService, ITeamService _teamService,
            IProjectService _projectService, ITaskService _taskService, IWorkLogService _workLogService) : base()
        {
            validations = validation;
            userService = _userService;
            teamService = _teamService;
            projectService = _projectService;
            taskService = _taskService;
            workLogService = _workLogService;
        }

        [Authorize(Policy = "WorkLogAccess")]
        [HttpGet("{taskId}")]
        public async Task<ActionResult> GetAll(int taskId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            ToDoTask task = await taskService.GetTaskById(taskId);
            validations.EnsureTaskExist(task);
            validations.CheckTaskAccess(currentUser, task);

            List<WorkLogResponseModel> workLogs = new List<WorkLogResponseModel>();

            foreach(WorkLog work in await workLogService.GetAll(taskId))
            {
                workLogs.Add(new WorkLogResponseModel()
                {
                    Id = work.Id,
                    UserId = work.UserId,
                    TaskId = work.ToDoTaskId,
                    WorkedHours = work.WorkedTime
                });
            }

            return Ok(workLogs);
        }

        [Authorize(Policy = "TaskAccess")]
        [HttpGet("Task/{taskId}/WorkLog/{workId}")]
        public async Task<WorkLogResponseModel> Get(int taskId, int workId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            WorkLog workLog = await workLogService.GetWorkLog(taskId, workId, currentUser);
            validations.EnsureWorkLogExist(workLog);

            return new WorkLogResponseModel()
            {
                Id = workLog.Id,
                UserId = workLog.UserId,
                TaskId = workLog.ToDoTaskId,
                WorkedHours = workLog.WorkedTime
            };
        }

        [Authorize(Policy = "WorkLogAccess")]
        [HttpPost("Task/{taskId}")]
        public async Task<ActionResult> Post(int taskId, WorkLogRequestModel model)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            bool isCreated = await workLogService.CreateWorkLog(currentUser, taskId, model.WorkedHours);

            if(isCreated && ModelState.IsValid)
            {
                return CreatedAtAction(nameof(Post), String.Format(Constants.Created, "WorkLog"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Policy = "WorkLogAccess")]
        [HttpPut("Task/{taskId}/WorkLog/{workLogId}")]
        public async Task<ActionResult<WorkLogResponseModel>> EditWorkLog(int taskId, int workLogId, WorkLogRequestModel model)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            if(await workLogService.EditWorkLog(taskId, workLogId, currentUser, model.WorkedHours))
            {
                return new WorkLogResponseModel()
                {
                    Id = workLogId,
                    TaskId = taskId,
                    UserId = currentUser.Id,
                    WorkedHours = model.WorkedHours
                };
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Policy = "WorkLogAccess")]
        [HttpDelete("Task/{taskId}/WorkLog/{workLogId}")]
        public async Task<ActionResult> Delete(int taskId, int workLogId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            if (await workLogService.DeleteWorkLog(taskId, workLogId, currentUser))
            {
                return Ok(String.Format(Constants.Deleted, "WorkLog"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }
    }
}
