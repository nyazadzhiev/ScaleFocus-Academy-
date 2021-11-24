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
    public class WorkLogController : ControllerBase
    {
        private readonly UserService userService;
        private readonly TeamService teamService;
        private readonly TaskService taskService;
        private readonly ProjectService projectService;
        private readonly WorkLogService workLogService;
        private Validation validations;

        public WorkLogController(DatabaseContext database) : base()
        {
            validations = new Validation(database);
            userService = new UserService(database, validations);
            teamService = new TeamService(database, userService, validations);
            projectService = new ProjectService(database, userService, teamService, validations);
            taskService = new TaskService(database, teamService, projectService, validations);
            workLogService = new WorkLogService(database, userService, taskService, validations);
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult> GetAll(int taskId)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

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
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

            WorkLog workLog = await workLogService.GetWorkLog(taskId, workId);
            validations.EnsureWorkLogExist(workLog);

            return Ok(workLog);
        }

        [HttpPost("Task/{taskId}")]
        public async Task<ActionResult> Post(int taskId, WorkLogRequestModel model)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

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
    }
}
