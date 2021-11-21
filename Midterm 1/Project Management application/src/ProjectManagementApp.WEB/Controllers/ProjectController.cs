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
            userService = new UserService(database);
            teamService = new TeamService(database, userService);
            projectService = new ProjectService(database, userService, teamService);
            validations = new Validation(database);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
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
            catch (UserNotFoundException)
            {
                return NotFound(Constants.UserNotFound);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseModel>> Get(int id)
        {
            try
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
            catch (UserNotFoundException)
            {
                return NotFound(Constants.UserNotFound);
            }
            catch (ProjectNotFoundException)
            {
                return NotFound(Constants.ProjectNotFound);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProjectRequestModel project)
        {
            try
            {
                User currentUser = await userService.GetCurrentUser(Request);

                validations.EnsureUserExist(currentUser);

                bool isCreated = await projectService.CreateProject(project.Title, currentUser);

                if (isCreated && ModelState.IsValid)
                {
                    Project projectFromDB = await projectService.GetProject(project.Title, currentUser);

                    return CreatedAtAction(nameof(Post), new { id = projectFromDB.Id }, Constants.CreatedProject);
                }
                else
                {
                    return BadRequest(Constants.FailedOperation);
                }
            }
            catch (UserNotFoundException)
            {
                return NotFound(Constants.UserNotFound);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectResponseModel>> Put(ProjectRequestModel project, int id)
        {
            try
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
            catch (UserNotFoundException)
            {
                return NotFound(Constants.UserNotFound);
            }
            catch (UnauthorizedUserException)
            {
                return Unauthorized(Constants.Unauthorized);
            }
            catch (ProjectExistException)
            {
                return BadRequest(Constants.ProjectExist);
            }
            catch (ProjectNotFoundException)
            {
                return NotFound(Constants.ProjectNotFound);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                User currentUser = await userService.GetCurrentUser(Request);

                validations.EnsureUserExist(currentUser);

                Project projectFromDB = await projectService.GetProject(id, currentUser);
                validations.EnsureProjectExist(projectFromDB);

                if (await projectService.DeleteProject(projectFromDB.Title, currentUser))
                {
                    return Ok(Constants.DeletedProject);
                }
                else
                {
                    return BadRequest(Constants.FailedOperation);
                }
            }
            catch (UserNotFoundException)
            {
                return NotFound(Constants.UserNotFound);
            }
            catch (ProjectNotFoundException)
            {
                return NotFound(Constants.ProjectNotFound);
            }
        }

        [HttpPost("{projectId}/Team/{teamId}")]
        public async Task<ActionResult> AddTeam(int projectId, int teamId)
        {
            try
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
            catch (UserNotFoundException)
            {
                return NotFound(Constants.UserNotFound);
            }
            catch (UnauthorizedUserException)
            {
                return Unauthorized(Constants.Unauthorized);
            }
            catch (UserExistException)
            {
                return BadRequest(Constants.UserInTeam);
            }
            catch (TeamNotFoundException)
            {
                return NotFound(Constants.TeamNotFound);
            }
            catch (ProjectNotFoundException)
            {
                return NotFound(Constants.ProjectNotFound);
            }
            catch (ProjectExistException)
            {
                return BadRequest(Constants.ProjectExist);
            }
        }

        [HttpDelete("{projectId}/Team/{teamId}")]
        public async Task<ActionResult> RemoveTeam(int projectId, int teamId)
        {
            try
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
            catch (UserNotFoundException)
            {
                return NotFound(Constants.UserNotFound);
            }
            catch (UnauthorizedUserException)
            {
                return Unauthorized(Constants.Unauthorized);
            }
            catch (TeamNotFoundException)
            {
                return NotFound(Constants.TeamNotFound);
            }
            catch (ProjectNotFoundException)
            {
                return NotFound(Constants.ProjectNotFound);
            }
        }
    }
}

