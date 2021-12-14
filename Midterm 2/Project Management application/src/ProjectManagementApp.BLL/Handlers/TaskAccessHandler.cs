using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using ProjectManagementApp.BLL.Contracts;
using ProjectManagementApp.BLL.Extensions;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Handlers
{
    public class TaskAccessHandler : AuthorizationHandler<TaskAccessRequirement>
    {
        private readonly IUserService userService;
        private readonly IProjectService projectService;
        private readonly ITaskService taskService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TaskAccessHandler(IUserService userService, IProjectService projectService, ITaskService taskService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.projectService = projectService;
            this.taskService = taskService;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TaskAccessRequirement requirement)
        {
            User user = await userService.GetUserByUsername(context.User.Identity.Name);

            string _taskId = httpContextAccessor.HttpContext.GetRouteValue("taskId").ToString();

            if (_taskId == null)
            {
                var body = await httpContextAccessor.HttpContext.Request.GetBodyAsync();
                _taskId = JObject.Parse(body)["taskId"].ToString();
            }

            int taskId = Int32.Parse(_taskId);

            ToDoTask task = await taskService.GetTaskById(taskId);

            if(task == null)
            {
                context.Fail();
                await Task.CompletedTask;
                return;
            }

            Project project = await projectService.GetProject(task.ProjectId, user);

            if (await userService.IsUserInRole(user.Id, "Admin") ||
                project.OwnerId == user.Id ||
                project.Teams.Any(t => t.Users.Any(u => u.Id == user.Id)))
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
            else
            {
                context.Fail();
                await Task.CompletedTask;
            }
        }
    }
}
