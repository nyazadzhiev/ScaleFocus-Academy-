using Moq;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManagmenetApp.Test.ProjectManagementApp.ServiceTest
{
    public class TeamServiceTests : ServicesTestBase
    {
        [Fact]
        public async Task CreateTeam_Should_Return_True_When_Team_Doesnt_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultTeam();
            var service = SetupMockedTeamService(mockedRepo);

            bool result = await service.CreateTeam("testname");

            Assert.True(result);
        }

        [Fact]
        public async Task GetTeam_Should_Return_Team_When_Team_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultTeam();
            var service = SetupMockedTeamService(mockedRepo);

            Team team = await service.GetTeam(this.defaultTeam.Id);

            Assert.NotNull(team);
        }

        [Fact]
        public async Task GetTeam_Should_Return_Null_When_Team_Doesnt_Exist()
        {
            var mockedRepo = SetupRepositoryReturningNullTeam();
            var service = SetupMockedTeamService(mockedRepo);

            Team team = await service.GetTeam(124);

            Assert.Null(team);
        }

        [Fact]
        public async Task EditTeam_Should_Return_True_When_Team_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultTeam();
            var service = SetupMockedTeamService(mockedRepo);

            bool result = await service.EditTeam(this.defaultTeam.Id, "NewName");

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTeam_Should_Return_True_When_Team_Exist()
        {
            var mockedRepo = SetupRepositoryReturningDefaultTeam();
            var service = SetupMockedTeamService(mockedRepo);

            bool result = await service.DeleteTeam(this.defaultTeam.Id);

            Assert.True(result);
        }
    }
}
