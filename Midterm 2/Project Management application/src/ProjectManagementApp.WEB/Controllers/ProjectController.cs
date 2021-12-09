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

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);

            List<ProjectResponseModel> projects = new List<ProjectResponseModel>();

            foreach (Project project in projectService.GetAll(currentUser))
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseModel>> Get(int id)
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);

            Project projectFromDB = await projectService.GetProject(id, currentUser);
            validations.EnsureProjectExist(projectFromDB);
            validations.CheckProjectAccess(currentUser, projectFromDB);

            return new ProjectResponseModel()
            {
                Id = projectFromDB.Id,
                Title = projectFromDB.Title,
                OwnerId = projectFromDB.OwnerId
            };
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProjectRequestModel project)
        {
            User currentUser = await userService.GetCurrentUser(User);
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

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectResponseModel>> Put(ProjectRequestModel project, int id)
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);

            if (await projectService.EditProject(id, project.Title, currentUser))
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);

            if (await projectService.DeleteProject(id, currentUser))
            {
                return Ok(String.Format(Constants.Deleted, "Project"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [HttpPost("{projectId}/Team/{teamId}")]
        public async Task<ActionResult> AddTeam(int projectId, int teamId)
        {
            User currentUser = await userService.GetCurrentUser(User);
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

        [HttpDelete("{projectId}/Team/{teamId}")]
        public async Task<ActionResult> RemoveTeam(int projectId, int teamId)
        {
            User currentUser = await userService.GetCurrentUser(User);
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

