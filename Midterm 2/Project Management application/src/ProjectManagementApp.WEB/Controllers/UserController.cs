using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagementApp.BLL.Services;
using ProjectManagementApp.DAL;
using ProjectManagementApp.WEB.Models.Requests;
using ProjectManagementApp.DAL.Entities;
using Common;
using ProjectManagementApp.WEB.Models.Responses;
using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.BLL.Exceptions;
using ProjectManagementApp.BLL.Contracts;

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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            List<UserResponseModel> users = new List<UserResponseModel>();

            foreach (User user in await userService.GetAllUsers())
            {
                users.Add(new UserResponseModel()
                {
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id
                });
            }

            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseModel>> Get(string id)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            User userFromDB = await userService.GetUserById(id);
            validations.EnsureUserExist(userFromDB);


            return new UserResponseModel()
            {
                Username = userFromDB.UserName,
                FirstName = userFromDB.FirstName,
                LastName = userFromDB.LastName,
                Id = userFromDB.Id
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Post(UserRequestModel user)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);

            bool isCreated = await userService.CreateUser(user.Username, user.Password, user.FirstName, user.LastName, user.Role);

            if (isCreated && ModelState.IsValid)
            {
                User userFromDB = await userService.GetUserByUsername(user.Username);

                validations.EnsureUserExist(userFromDB);

                return CreatedAtAction(nameof(Post), new { id = userFromDB.Id }, String.Format(Constants.Created, "User"));
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseModel>> Put(EditUserRequestModel user, string id)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);
            //validations.CheckRole(currentUser);

            if (await userService.EditUser(id, user.Username, user.Password, user.FirstName, user.LastName))
            {
                User edited = await userService.GetUserByUsername(user.Username);

                return new UserResponseModel()
                {
                    Username = edited.UserName,
                    FirstName = edited.FirstName,
                    LastName = edited.LastName,
                    Id = edited.Id,
                };
            }
            else
            {
                return BadRequest(Constants.FailedOperation);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            User currentUser = await userService.GetCurrentUserAsync(User);
            validations.LoginCheck(currentUser);
            //validations.CheckRole(currentUser);

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
