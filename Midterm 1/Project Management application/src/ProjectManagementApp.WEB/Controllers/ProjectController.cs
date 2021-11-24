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
    public class ProjectController : ControllerBase
    {
        private readonly UserService userService;
        private readonly TeamService teamService;
        private readonly ProjectService projectService;
        private Validation validations;

        public ProjectController(DatabaseContext database) : base()
        {
            userService = new UserService(database, validations);
            teamService = new TeamService(database, userService, validations);
            projectService = new ProjectService(database, userService, teamService, validations);
            validations = new Validation(database);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

            List<ProjectResponseModel> projects = new List<ProjectResponseModel>();

            foreach (Project project in await projectService.GetAll(currentUser))
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
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

            Project projectFromDB = await projectService.GetProject(id, currentUser);
            validations.EnsureProjectExist(projectFromDB);


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
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

            bool isCreated = await projectService.CreateProject(project.Title, currentUser);

            if (isCreated && ModelState.IsValid)
            {
                Project projectFromDB = await projectService.GetProject(project.Title, currentUser);

                return CreatedAtAction(nameof(Post), new { id = projectFromDB.Id }, Constants.Created);
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectResponseModel>> Put(ProjectRequestModel project, int id)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

            Project projectFromDB = await projectService.GetProject(id, currentUser);
            validations.EnsureProjectExist(projectFromDB);

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
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);

            Project projectFromDB = await projectService.GetProject(id, currentUser);
            validations.EnsureProjectExist(projectFromDB);

            if (await projectService.DeleteProject(projectFromDB.Title, currentUser))
            {
                return Ok(Constants.Deleted);
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [HttpPost("{projectId}/Team/{teamId}")]
        public async Task<ActionResult> AddTeam(int projectId, int teamId)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);
            validations.CheckRole(currentUser);

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
            User currentUser = await userService.GetCurrentUser(Request);

            validations.EnsureUserExist(currentUser);

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

