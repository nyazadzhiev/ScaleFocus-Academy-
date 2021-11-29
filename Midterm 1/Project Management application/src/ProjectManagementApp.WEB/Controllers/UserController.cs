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
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private IValidationService validations;

        public UserController(IValidationService validation, IUserService _userService) : base()
        {
            validations = validation;
            userService = _userService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);
            validations.CheckRole(currentUser);

            List<UserResponseModel> users = new List<UserResponseModel>();

            foreach (User user in await userService.GetAllUsers())
            {
                users.Add(new UserResponseModel()
                {
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsAdmin = user.IsAdmin,
                    Id = user.Id
                });
            }

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseModel>> Get(int id)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);
            validations.CheckRole(currentUser);

            User userFromDB = await userService.GetUser(id);
            validations.EnsureUserExist(userFromDB);


            return new UserResponseModel()
            {
                Username = userFromDB.Username,
                FirstName = userFromDB.FirstName,
                LastName = userFromDB.LastName,
                IsAdmin = userFromDB.IsAdmin,
                Id = userFromDB.Id
            };
        }

        [HttpPost]
        public async Task<ActionResult> Post(UserRequestModel user)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);
            validations.CheckRole(currentUser);

            bool isCreated = await userService.CreateUser(user.Username, user.Password, user.FirstName, user.LastName, user.IsAdmin, currentUser);

            if (isCreated && ModelState.IsValid)
            {
                User userFromDB = await userService.GetUser(user.Username);

                validations.EnsureUserExist(userFromDB);

                return CreatedAtAction(nameof(Post), new { id = userFromDB.Id }, String.Format(Constants.Created, "User"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseModel>> Put(UserRequestModel user, int id)
        {
            User currentUser = await userService.GetCurrentUser(Request);
            validations.EnsureUserExist(currentUser);
            validations.CheckRole(currentUser);

            if (await userService.EditUser(id, user.Username, user.Password, user.FirstName, user.LastName))
            {
                User edited = await userService.GetUser(user.Username);

                return new UserResponseModel()
                {
                    Username = edited.Username,
                    FirstName = edited.FirstName,
                    LastName = edited.LastName,
                    IsAdmin = edited.IsAdmin,
                    Id = edited.Id,
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
            validations.CheckRole(currentUser);

            if (await userService.DeleteUser(id))
            {
                return Ok(String.Format(Constants.Deleted, "User"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }
    }
}
