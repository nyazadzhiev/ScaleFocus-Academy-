using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using ProjectManagementApp.BLL.Exceptions;
using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProjectManagmenetApp.Test.ProjectManagementApp.ServiceTest
{
    public class ValidationServiceTests : ServicesTestBase
    {
        [Fact]
        public void EnsureUserExist_Should_Throw_UserNotFoundException_When_User_Doesnt_Exist()
        {
            var validation = SetupMockedValidationService();

            Assert.Throws<UserNotFoundException>(() => validation.EnsureUserExist(null));
        }

        [Fact]
        public void EnsureProjectExist_Should_Throw_ProjectNotFoundException_When_Project_Doesnt_Exist()
        {
            var validation = SetupMockedValidationService();

            Assert.Throws<ProjectNotFoundException>(() => validation.EnsureProjectExist(null));
        }

        [Fact]
        public void EnsureTeamExist_Should_Throw_TeamNotFoundException_When_Team_Doesnt_Exist()
        {
            var validation = SetupMockedValidationService();

            Assert.Throws<TeamNotFoundException>(() => validation.EnsureTeamExist(null));
        }

        [Fact]
        public void EnsureTaskExist_Should_Throw_TaskNotFoundException_When_Task_Doesnt_Exist()
        {
            var validation = SetupMockedValidationService();

            Assert.Throws<TaskNotFoundException>(() => validation.EnsureTaskExist(null));
        }

        [Fact]
        public void EnsureWorkLogExist_Should_Throw_WorkLogNotFoundException_When_WorkLog_Doesnt_Exist()
        {
            var validation = SetupMockedValidationService();

            Assert.Throws<WorkLogNotFoundException>(() => validation.EnsureWorkLogExist(null));
        }

        [Fact]
        public void CheckRole_Should_Throw_UnauthorizedUserException_When_User_IsNotAdmin()
        {
            var validation = SetupMockedValidationService();

            Assert.Throws<UnauthorizedUserException>(() => validation.CheckRole(this.regularUser));
        }

        [Fact]
        public void CanAddToTeam_Should_Throw_UserExistException_When_User_Exist()
        {
            var validation = SetupMockedValidationService();

            Assert.Throws<UserExistException>(() => validation.CanAddToTeam(regularTeam, regularUser));
        }

        [Fact]
        public void CanAddToProject_Should_Throw_TeamExistException_When_User_Exist()
        {
            var validation = SetupMockedValidationService();

            Assert.Throws<TeamExistException>(() => validation.CanAddToProject(regularProject, regularTeam));
        }

        [Fact]
        public void CheckProjectAcces_Should_Throw_UnauthorizedUserException_When_User_Unautharized()
        {
            var validation = SetupMockedValidationService();

            Assert.Throws<UnauthorizedUserException>(() => validation.CheckProjectAccess(adminUser, regularProject));
        }

        [Fact]
        public void CheckTaskAcces_Should_Throw_UnauthorizedUserException_When_User_Unautharized()
        {
            var validation = SetupMockedValidationService();

            Assert.Throws<UnauthorizedUserException>(() => validation.CheckTaskAccess(adminUser, regularTask));
        }

        [Fact]
        public void CheckUsername_Should_UserExistException_When_User_Unautharized()
        {
            var options = SetupDbOptions();

            using(var context = new DatabaseContext(options))
            {
                context.Users.Add(adminUser);
                context.SaveChanges();

                var validation = new ValidationService(context);

                Assert.Throws<UserExistException>(() => validation.CheckUsername(adminUser.Username));

                context.Users.Remove(adminUser);
                context.SaveChanges();
            }
        }

        [Fact]
        public void CheckTeamName_Should_UserExistException_When_User_Unautharized()
        {
            var options = SetupDbOptions();

            using (var context = new DatabaseContext(options))
            {
                context.Teams.Add(regularTeam);
                context.SaveChanges();

                var validation = new ValidationService(context);

                Assert.Throws<TeamExistException>(() => validation.CheckTeamName(regularTeam.Name));

                context.Teams.Remove(regularTeam);
                context.SaveChanges();
            }
        }

        [Fact]
        public void CheckProjectName_Should_UserExistException_When_User_Unautharized()
        {
            var options = SetupDbOptions();

            using (var context = new DatabaseContext(options))
            {
                context.Projects.Remove(regularProject);

                context.Projects.Add(regularProject);
                context.SaveChanges();

                var validation = new ValidationService(context);

                Assert.Throws<ProjectExistException>(() => validation.CheckProjectName(regularProject.Title));

                context.Projects.Remove(regularProject);
                context.SaveChanges();
            }
        }

        [Fact]
        public void CheckTaskName_Should_UserExistException_When_User_Unautharized()
        {
            var options = SetupDbOptions();

            using (var context = new DatabaseContext(options))
            {
                context.ToDoTasks.Add(regularTask);
                context.SaveChanges();

                var validation = new ValidationService(context);

                Assert.Throws<TaskExistException>(() => validation.CheckTaskName(regularTask.Title));

                context.ToDoTasks.Remove(regularTask);
                context.SaveChanges();
            }
        }
    }
}
