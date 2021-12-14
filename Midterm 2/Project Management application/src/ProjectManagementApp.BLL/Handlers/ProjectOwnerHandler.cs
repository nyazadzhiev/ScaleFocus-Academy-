using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ProjectManagementApp.BLL.Contracts;
using ProjectManagementApp.DAL.Entities;
using ProjectManagementApp.BLL.Extensions;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProjectManagementApp.BLL.Handlers
{
    public class ProjectOwnerHandler : AuthorizationHandler<ProjectOwnerRequirement>
    {
        private readonly IUserService userService;
        private readonly IProjectService projectService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProjectOwnerHandler(IUserService userService,IProjectService projectService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.projectService = projectService;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ProjectOwnerRequirement requirement)
        {
            User user = await userService.GetUserByUsername(context.User.Identity.Name);
            string _projectId = httpContextAccessor.HttpContext.GetRouteValue("projectId").ToString();

            if(_projectId == null)
            {
                var body = await httpContextAccessor.HttpContext.Request.GetBodyAsync();
                _projectId = JObject.Parse(body)["projectId"].ToString();
            }

            int projectId = Int32.Parse(_projectId);

            Project project = await projectService.GetProject(projectId, user);

            if(project == null)
            {
                context.Fail();
                await Task.CompletedTask;
            }
            else if(await userService.IsUserInRole(user.Id, "Admin") ||
                project.OwnerId == user.Id)
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
