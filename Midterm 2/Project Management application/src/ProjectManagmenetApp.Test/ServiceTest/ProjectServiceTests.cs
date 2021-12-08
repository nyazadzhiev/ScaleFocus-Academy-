using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManagmenetApp.Test.ProjectManagementApp.ServiceTest
{
    public class ProjectServiceTests : ServicesTestBase
    {
        [Fact]
        public async Task CreateProject_Should_Return_True_When_Project_Doesnt_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultProject();
            var service = SetupMockedProjectService(mockedRepo);

            bool result = await service.CreateProject("title", regularUser);

            Assert.True(result);
        }

        [Fact]
        public async Task GetProject_Should_Return_Project_When_Project_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultProject();
            var service = SetupMockedProjectService(mockedRepo);

            Project project = await service.GetProject(this.defaultProject.Id, this.regularUser);

            Assert.NotNull(project);
        }

        [Fact]
        public async Task GetProject_Should_Return_Null_When_Project_Doesnt_Exist()
        {
            var mockedRepo = SetupRepositoryReturningNullProject();
            var service = SetupMockedProjectService(mockedRepo);

            Project project = await service.GetProject(134, this.regularUser);

            Assert.Null(project);
        }

        [Fact]
        public async Task EditProject_Should_Return_True_When_Project_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultProject();
            var service = SetupMockedProjectService(mockedRepo);

            bool result = await service.EditProject(defaultProject.Id, "NewTitle", regularUser);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProject_Should_Return_True_When_Project_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultProject();
            var service = SetupMockedProjectService(mockedRepo);

            bool result = await service.DeleteProject(defaultProject.Id, regularUser);

            Assert.True(result);
        }
    }
}
