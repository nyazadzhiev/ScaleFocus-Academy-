using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManagmenetApp.Test.ProjectManagementApp.ServiceTest
{
    public class TaskServiceTests : ServicesTestBase
    {
        [Fact]
        public async Task CreateTask_Should_Return_True_When_Task_Doesnt_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultTask();
            var service = SetupMockedTaskService(mockedRepo);

            bool result = await service.CreateTask("title", "desc", false, regularProject.Id, adminUser, regularUser.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task GetTask_Should_Return_Task_When_Task_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultTask();
            var service = SetupMockedTaskService(mockedRepo);

            ToDoTask task = await service.GetTask(defaultTask.Id);

            Assert.NotNull(task);
        }

        [Fact]
        public async Task GetTask_Should_Return_Null_When_Task_Doesnt_Exist()
        {
            var mockedRepo = SetupRepositoryReturningNullTask();
            var service = SetupMockedTaskService(mockedRepo);

            ToDoTask task = await service.GetTask(152);

            Assert.Null(task);
        }

        [Fact]
        public async Task EditTask_Should_Return_True_When_Task_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultTask();
            var service = SetupMockedTaskService(mockedRepo);

            bool result = await service.EditTask(regularTask.Id, regularProject.Id, regularUser, "newtitle", "newdesc", true);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTask_Should_Return_True_When_Task_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultTask();
            var service = SetupMockedTaskService(mockedRepo);

            bool result = await service.DeleteTask(regularTask.Id, regularProject.Id, regularUser);

            Assert.True(result);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_True_When_Task_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultTask();
            var service = SetupMockedTaskService(mockedRepo);

            bool result = await service.ChangeStatus(regularTask.Id, regularProject.Id, regularUser);

            Assert.True(result);
        }
    }
}
