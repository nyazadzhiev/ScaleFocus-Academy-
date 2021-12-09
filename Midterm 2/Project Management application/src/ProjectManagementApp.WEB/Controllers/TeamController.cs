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
    public class TeamController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITeamService teamService;
        private IValidationService validations;

        public TeamController(IValidationService validation, IUserService _userService, ITeamService _teamService) : base()
        {
            validations = validation;
            userService = _userService;
            teamService = _teamService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);
            //validations.CheckRole(currentUser);

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

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamResponseModel>> Get(int id)
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);
            //validations.CheckRole(currentUser);

            Team teamFromDB = await teamService.GetTeam(id);
            validations.EnsureTeamExist(teamFromDB);

            return new TeamResponseModel()
            {
                Name = teamFromDB.Name,
                Id = teamFromDB.Id
            };
        }

        [HttpPost]
        public async Task<ActionResult> Post(TeamRequestModel team)
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);
            //validations.CheckRole(currentUser);

            bool isCreated = await teamService.CreateTeam(team.Name);

            if (isCreated && ModelState.IsValid)
            {
                Team teamFromDB = await teamService.GetTeam(team.Name);

                return CreatedAtAction(nameof(Post), new { id = teamFromDB.Id }, String.Format(Constants.Created, "Team"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TeamResponseModel>> Put(TeamRequestModel team, int id)
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);
            //validations.CheckRole(currentUser);

            if (await teamService.EditTeam(id, team.Name))
            {
                Team edited = await teamService.GetTeam(team.Name);

                return new TeamResponseModel()
                {
                    Name = edited.Name
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
            //validations.CheckRole(currentUser);

            if (await teamService.DeleteTeam(id))
            {
                return Ok(String.Format(Constants.Deleted, "Team"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [HttpPost("{teamId}/User/{userId}")]
        public async Task<ActionResult> AddUser(int teamId, string userId)
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);
            //validations.CheckRole(currentUser);

            if (await teamService.AddUser(teamId, userId))
            {
                return Ok(Constants.UserAddedToTeam);
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [HttpDelete("{teamId}/User/{userId}")]
        public async Task<ActionResult> RemoveUser(int teamId, string userId)
        {
            User currentUser = await userService.GetCurrentUser(User);
            validations.LoginCheck(currentUser);
            //validations.CheckRole(currentUser);

            if (await teamService.RemoveUser(teamId, userId))
            {
                return Ok(Constants.UserRemovedFromTeam);
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }
    }
}

