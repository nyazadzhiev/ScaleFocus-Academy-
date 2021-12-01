using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectManagementApp.BLL.Exceptions;
using ProjectManagementApp.BLL.Services;
using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.DAL.Entities;
using ProjectManagementApp.DAL.Models.Requests;
using ProjectManagementApp.WEB.Auth;
using ProjectManagementApp.WEB.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManagmenetApp.Test.ControllerTest
{
    public class UserControllerTests
    {
        [Fact]
        public void Post_Should_Return_401_When_Unauthorized()
        {
            var authProvider = new Mock<IAuthProvider>();
            authProvider.Setup(ap => ap.GetCurrentUser(It.IsAny<HttpRequest>())).Returns(Task.FromResult(new User() { IsAdmin = false }));
            var validationMock = new Mock<IValidationService>();
            validationMock.Setup(v => v.CheckRole(It.IsAny<User>())).Throws(new UnauthorizedUserException(Constants.Unauthorized));
            var userService = new Mock<IUserService>();
            var controller = new UserController(validationMock.Object, userService.Object, authProvider.Object);
            var model = new UserRequestModel { Username = "test1", Password = "test1" };

            var result = controller.Post(model);

            Assert.Equal(Constants.Unauthorized, result.Exception.InnerException.Message);
        }

        [Fact]
        public async Task Post_Should_Return_Ok_When_Authorized()
        {
            var authProvider = new Mock<IAuthProvider>();
            authProvider.Setup(ap => ap.GetCurrentUser(It.IsAny<HttpRequest>())).Returns(Task.FromResult(new User() { IsAdmin = true }));
            var validationMock = new Mock<IValidationService>();
            var userService = new Mock<IUserService>();
            userService.Setup(u => u.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(true));
            userService.Setup(u => u.GetUser(It.IsAny<string>())).Returns(Task.FromResult(new User() { }));
            var controller = new UserController(validationMock.Object, userService.Object, authProvider.Object);
            var model = new UserRequestModel { Username = "test1", Password = "test1" };

            var result = await controller.Post(model);

            result.ToString().Equals(HttpStatusCode.OK);
        }
    }
}
