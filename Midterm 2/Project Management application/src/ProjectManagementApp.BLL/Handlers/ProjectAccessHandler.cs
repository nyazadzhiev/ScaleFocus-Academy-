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
    public class ProjectAccessHandler : AuthorizationHandler<ProjectAccessRequirement>
    {
        private readonly IUserService userService;
        private readonly IProjectService projectService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProjectAccessHandler(IUserService userService, IProjectService projectService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.projectService = projectService;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ProjectAccessRequirement requirement)
        {
            User user = await userService.GetUserByUsername(context.User.Identity.Name);
            string _projectId = httpContextAccessor.HttpContext.GetRouteValue("projectId").ToString();

            if (_projectId == null)
            {
                var body = await httpContextAccessor.HttpContext.Request.GetBodyAsync();
                _projectId = JObject.Parse(body)["projectId"].ToString();
            }

            int projectId = Int32.Parse(_projectId);

            Project project = await projectService.GetProject(projectId, user);

            if (project == null)
            {
                context.Fail();
                await Task.CompletedTask;
            }
            else if (await userService.IsUserInRole(user.Id, "Admin") ||
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
