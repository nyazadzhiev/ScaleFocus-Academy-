using Common;
using Microsoft.Extensions.Configuration;
using Moq;
using ProjectManagementApp.BLL.Exceptions;
using ProjectManagementApp.BLL.Services;
using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using ProjectManagementApp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManagmenetApp.Test.ProjectManagementApp.ServiceTest
{
    public class UserServiceTest : ServicesTestBase
    {
        [Fact]
        public async Task CreateUser_Should_Return_True_When_User_Doesnt_Exist()
        {
            var mockedRepo = SetupRepositoryReturningAdminUser();
            var service = SetupMockedUserService(mockedRepo);

            bool result = await service.CreateUser("test111", "test1", "test1", "test1", false);

            Assert.True(result);
        }

        [Fact]
        public async Task GetUser_Should_Return_User()
        {
            var mockedRepo = SetupRepositoryReturningDefaultUser();
            var service = SetupMockedUserService(mockedRepo);

            User user = await service.GetUser(this.defaultUser.Id);

            Assert.NotNull(user);
        }

        [Fact]
        public async Task GetUser_Should_Return_Null_When_User_Doesnt_Exist()
        {
            var mockedRepo = SetupRepositoryReturningNull();
            var service = SetupMockedUserService(mockedRepo);

            User user = await service.GetUser(124);

            Assert.Null(user);
        }

        [Fact]
        public async Task EditUser_Should_Return_True_When_User_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultUser();
            var service = SetupMockedUserService(mockedRepo);

            bool result = await service.EditUser(this.defaultUser.Id, "newName", "newPass", "NewName", "NewName");

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUser_Should_Return_True_When_User_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultUser();
            var service = SetupMockedUserService(mockedRepo);

            bool result = await service.DeleteUser(this.defaultUser.Id);

            Assert.True(result);
        }

    }
}
