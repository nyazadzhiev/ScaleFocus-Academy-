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
    public class TeamController : ControllerBase
    {
        private readonly UserService userService;
        private readonly TeamService teamService;
        private Validation validations;

        public TeamController(DatabaseContext database) : base()
        {
            userService = new UserService(database);
            teamService = new TeamService(database, userService);
            validations = new Validation(database);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                User currentUser = await userService.GetCurrentUser(Request);
                validations.EnsureUserExist(currentUser);
                validations.CheckRole(currentUser);

                List<TeamResponseModel> teams = new List<TeamResponseModel>();

                foreach (Team team in await teamService.GetAll())
                {
                    teams.Add(new TeamResponseModel()
                    {
                        Name = team.Name,
                        Id = team.Id
                    });
                }

                return Ok(teams);
            }
            catch (UserNotFoundException)
            {
                return NotFound(Constants.UserNotFound);
            }
            catch (UnauthorizedUserException)
            {
                return Unauthorized(Constants.Unauthorized);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamResponseModel>> Get(int id)
        {
            try
            {
                User currentUser = await userService.GetCurrentUser(Request);
                validations.EnsureUserExist(currentUser);
                validations.CheckRole(currentUser);

                Team teamFromDB = await teamService.GetTeam(id);

                validations.EnsureTeamExist(teamFromDB);


                return new TeamResponseModel()
                {
                    Name = teamFromDB.Name,
                    Id = teamFromDB.Id
                };
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
        }

        [HttpPost]
        public async Task<ActionResult> Post(TeamRequestModel team)
        {
            try
            {
                User currentUser = await userService.GetCurrentUser(Request);

                validations.EnsureUserExist(currentUser);
                validations.CheckRole(currentUser);

                bool isCreated = await teamService.CreateTeam(team.Name);

                if (isCreated && ModelState.IsValid)
                {
                    Team teamFromDB = await teamService.GetTeam(team.Name);


                    return CreatedAtAction(nameof(Post), new { id = teamFromDB.Id }, Constants.CreatedTeam);
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
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TeamResponseModel>> Put(TeamRequestModel team, int id)
        {
            try
            {
                User currentUser = await userService.GetCurrentUser(Request);
                validations.EnsureUserExist(currentUser);
                validations.CheckRole(currentUser);

                Team teamFromDB = await teamService.GetTeam(id);

                validations.EnsureTeamExist(teamFromDB);

                if (await teamService.EditTeam(id, team.Name))
                {
                    Team edited = await teamService.GetTeam(team.Name);

                    return new TeamResponseModel()
                    {
                        Name = team.Name,
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
            catch (UserExistException)
            {
                return BadRequest(Constants.UserExist);
            }
            catch (TeamNotFoundException)
            {
                return NotFound(Constants.TeamNotFound);
            }
            catch (TeamExistException)
            {
                return BadRequest(Constants.TeamExist);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                User currentUser = await userService.GetCurrentUser(Request);

                validations.EnsureUserExist(currentUser);
                validations.CheckRole(currentUser);

                Team teamFromDB = await teamService.GetTeam(id);
                validations.EnsureTeamExist(teamFromDB);

                if (await teamService.DeleteTeam(teamFromDB.Name))
                {
                    return Ok(Constants.DeletedTeam);
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
        }

        [HttpPost("{teamId}/User/{userId}")]
        public async Task<ActionResult> AddUser(int teamId, int userId)
        {
            try
            {
                User currentUser = await userService.GetCurrentUser(Request);

                validations.EnsureUserExist(currentUser);
                validations.CheckRole(currentUser);

                if(await teamService.AddUser(teamId, userId))
                {
                    return Ok(Constants.UserAddedToTeam);
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
        }

        [HttpDelete("{teamId}/User/{userId}")]
        public async Task<ActionResult> RemoveUser(int teamId, int userId)
        {
            try
            {
                User currentUser = await userService.GetCurrentUser(Request);

                validations.EnsureUserExist(currentUser);
                validations.CheckRole(currentUser);

                if (await teamService.RemoveUser(teamId, userId))
                {
                    return Ok(Constants.UserRemovedFromTeam);
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
        }
    }
}

