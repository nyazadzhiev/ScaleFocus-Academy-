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
    public class ProjectController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITeamService teamService;
        private readonly IProjectService projectService;
        private IValidationService validations;

        public ProjectController(IValidationService validation, IUserService _userService, ITeamService _teamService, IProjectService _projectService) : base()
        {
            validations = validation;
            userService = _userService;
            teamService = _teamService;
            projectService = _projectService;
        }

        [Authorize]
        [HttpGet("MyProjects")]
        public async Task<ActionResult> GetMyProjects()
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            List<ProjectResponseModel> projects = new List<ProjectResponseModel>();

            foreach (Project project in projectService.GetMyProjects(currentUser))
            {
                projects.Add(new ProjectResponseModel()
                {
                    Id = project.Id,
                    Title = project.Title,
                    OwnerId = project.OwnerId
                });
            }

            return Ok(projects);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAllProjects()
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            List<ProjectResponseModel> projects = new List<ProjectResponseModel>();

            foreach (Project project in projectService.GetAllProjects())
            {
                projects.Add(new ProjectResponseModel()
                {
                    Id = project.Id,
                    Title = project.Title,
                    OwnerId = project.OwnerId
                });
            }

            return Ok(projects);
        }

        [Authorize(Policy = "ProjectOwner")]
        [HttpGet("{projectId}")]
        public async Task<ActionResult<ProjectResponseModel>> Get(int projectId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            Project projectFromDB = await projectService.GetProject(projectId, currentUser);
            validations.EnsureProjectExist(projectFromDB);

            return new ProjectResponseModel()
            {
                Id = projectFromDB.Id,
                Title = projectFromDB.Title,
                OwnerId = projectFromDB.OwnerId
            };
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post(ProjectRequestModel project)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            bool isCreated = await projectService.CreateProject(project.Title, currentUser);

            if (isCreated && ModelState.IsValid)
            {
                Project projectFromDB = await projectService.GetProject(project.Title, currentUser);

                return CreatedAtAction(nameof(Post), new { id = projectFromDB.Id }, String.Format(Constants.Created, "Project"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Policy = "ProjectOwner")]
        [HttpPut("{projectId}")]
        public async Task<ActionResult<ProjectResponseModel>> Put(EditProjectRequestModel project, int projectId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            if (await projectService.EditProject(projectId, project.Title, currentUser))
            {
                Project edited = await projectService.GetProject(project.Title, currentUser);

                return new ProjectResponseModel()
                {
                    Id = edited.Id,
                    Title = edited.Title,
                    OwnerId = edited.OwnerId
                };
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Policy = "ProjectOwner")]
        [HttpDelete("{projectId}")]
        public async Task<ActionResult> Delete(int projectId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            if (await projectService.DeleteProject(projectId, currentUser))
            {
                return Ok(String.Format(Constants.Deleted, "Project"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Policy = "ProjectOwner")]
        [HttpPost("{projectId}/Team/{teamId}")]
        public async Task<ActionResult> AddTeam(int projectId, int teamId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            if (await projectService.AddTeam(projectId, teamId, currentUser))
            {
                return Ok(Constants.TeamAddedToProject);
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Policy = "ProjectOwner")]
        [HttpDelete("{projectId}/Team/{teamId}")]
        public async Task<ActionResult> RemoveTeam(int projectId, int teamId)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            if (await projectService.RemoveTeam(projectId, teamId, currentUser))
            {
                return Ok(Constants.TeamRemovedFromProject);
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }
    }
}

