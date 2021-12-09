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

        [HttpGet("{taskId}")]
        public async Task<ActionResult> GetAll(int taskId)
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);

            ToDoTask task = await taskService.GetTask(taskId);
            validations.EnsureTaskExist(task);
            validations.CheckTaskAccess(currentUser, task);

            List<WorkLogResponseModel> workLogs = new List<WorkLogResponseModel>();

            foreach(WorkLog work in await workLogService.GetAll(taskId))
            {
                workLogs.Add(new WorkLogResponseModel()
                {
                    UserId = work.UserId,
                    TaskId = work.ToDoTaskId,
                    WorkedHours = work.WorkedTime
                });
            }

            return Ok(workLogs);
        }

        [HttpGet("Task/{taskId}/WorkLog/{workId}")]
        public async Task<ActionResult> Get(int taskId, int workId)
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);

            WorkLog workLog = await workLogService.GetWorkLog(taskId, workId, currentUser);
            validations.EnsureWorkLogExist(workLog);

            return Ok(workLog);
        }

        [HttpPost("Task/{taskId}")]
        public async Task<ActionResult> Post(int taskId, WorkLogRequestModel model)
        {
            User currentUser = await userService.GetCurrentUser(User);
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

        [HttpPut("Task/{taskId}/WorkLog/{workLofId}")]
        public async Task<ActionResult<WorkLogResponseModel>> EditWorkLog(int taskId, int workLogId, WorkLogRequestModel model)
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);

            if(await workLogService.EditWorkLog(taskId, workLogId, currentUser))
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

        [HttpDelete("Task/{taskId}/WorkLog/{workLofId}")]
        public async Task<ActionResult> Delete(int taskId, int workLogId)
        {
            User currentUser = await userService.GetCurrentUser(User);
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
