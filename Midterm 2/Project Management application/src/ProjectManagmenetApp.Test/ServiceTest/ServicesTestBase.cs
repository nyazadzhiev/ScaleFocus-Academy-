using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using ProjectManagementApp.BLL.Services;
using ProjectManagementApp.BLL.Validations;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using ProjectManagementApp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmenetApp.Test.ProjectManagementApp.ServiceTest
{
    public class ServicesTestBase
    {
        public User defaultUser { get; set; }
        public User adminUser { get; set; }
        public User regularUser { get; set; }
        public Team defaultTeam { get; set; }
        public Team regularTeam { get; set; }
        public Project defaultProject { get; set; }
        public Project regularProject { get; set; }
        public ToDoTask defaultTask { get; set; }
        public ToDoTask regularTask { get; set; }

        public ServicesTestBase()
        {
            this.defaultUser = new User();

            this.adminUser = new User()
            {
                Username = "admin",
                Password = "adminpassword",
                FirstName = "Admin",
                LastName = "Admin",
                IsAdmin = true
            };

            this.regularUser = new User()
            {
                Username = "regularUser",
                Password = "userpassword",
                FirstName = "User",
                LastName = "User",
                IsAdmin = false,
                Id = 1,
                Teams = { },
                WorkLogs = {},
                ToDoTasks = {}
            };

            this.defaultTeam = new Team();

            this.regularTeam = new Team()
            {
                Name = "testteam",
                Id = 1, 
                Users = { regularUser },
                Projects = { }
            };

            regularUser.Teams.Add(regularTeam);

            this.defaultProject = new Project();

            this.regularProject = new Project()
            {
                Title = "regular", 
                Teams = { regularTeam },
                OwnerId = regularUser.Id
            };

            regularTeam.Projects.Add(regularProject);

            this.defaultTask = new ToDoTask();

            this.regularTask = new ToDoTask()
            {
                Title = "testets",
                Description = "desc",
                IsCompleted = false, 
                AsigneeId = regularUser.Id
            };
        }

        private Mock<IUserRepository> SetupDefaultMockedUserRepository()
        {
            return new Mock<IUserRepository>();
        }

        private Mock<ITeamRepository> SetupDefaultMockedTeamRepository()
        {
            return new Mock<ITeamRepository>();
        }

        private Mock<IProjectRepository> SetupDefaultMockedProjectRepository()
        {
            return new Mock<IProjectRepository>();
        }

        private Mock<ITaskRepository> SetupDefaultMockedTaskRepository()
        {
            return new Mock<ITaskRepository>();
        }

        protected Mock<ITaskRepository> SetupRepositoryReturningDefaultTask()
        {
            var mockedRepo = SetupDefaultMockedTaskRepository();
            mockedRepo.Setup(t => t.GetTaskAsync(It.IsAny<int>())).Returns(Task.FromResult(this.defaultTask));

            return mockedRepo;
        }

        protected Mock<ITaskRepository> SetupRepositoryReturningNullTask()
        {
            var mockedRepo = SetupDefaultMockedTaskRepository();
            mockedRepo.Setup(t => t.GetTaskAsync(It.IsAny<int>())).Returns(Task.FromResult<ToDoTask> (null));

            return mockedRepo;
        }

        protected TaskService SetupMockedTaskService(Mock<ITaskRepository> mockedRepo)
        {
            var validationMock = new Mock<IValidationService>();
            var projectServiceMock = new Mock<IProjectService>();
            var service = new TaskService(mockedRepo.Object, projectServiceMock.Object, validationMock.Object);

            return service;
        }

        protected Mock<IProjectRepository> SetupRepositoryReturningDefaultProject()
        {
            var mockedRepo = SetupDefaultMockedProjectRepository();
            mockedRepo.Setup(p => p.GetProject(It.IsAny<int>())).Returns(Task.FromResult(this.defaultProject));

            return mockedRepo;
        }

        protected Mock<IProjectRepository> SetupRepositoryReturningNullProject()
        {
            var mockedRepo = SetupDefaultMockedProjectRepository();
            mockedRepo.Setup(p => p.GetProject(It.IsAny<int>())).Returns(Task.FromResult<Project> (null));

            return mockedRepo;
        }

        protected ProjectService SetupMockedProjectService(Mock<IProjectRepository> mockedRepo)
        {
            var validationMock = new Mock<IValidationService>();
            var teamServiceMock = new Mock<ITeamService>();

            ProjectService service = new ProjectService(mockedRepo.Object, teamServiceMock.Object, validationMock.Object);

            return service;
        }

        protected Mock<ITeamRepository> SetupRepositoryReturningDefaultTeam()
        {
            var mockedRepo = SetupDefaultMockedTeamRepository();
            mockedRepo.Setup(t => t.GetTeamAsync(It.IsAny<int>())).Returns(Task.FromResult(this.defaultTeam));

            return mockedRepo;
        }

        protected Mock<ITeamRepository> SetupRepositoryReturningNullTeam()
        {
            var mockedRepo = SetupDefaultMockedTeamRepository();
            mockedRepo.Setup(t => t.GetTeamAsync(It.IsAny<int>())).Returns(Task.FromResult<Team> (null));

            return mockedRepo;
        }

        protected TeamService SetupMockedTeamService(Mock<ITeamRepository> mockedRepo)
        {
            var validationMock = new Mock<IValidationService>();
            var userServiceMock = new Mock<IUserService>();

            TeamService service = new TeamService(mockedRepo.Object, userServiceMock.Object, validationMock.Object);

            return service;
        }

        protected Mock<IUserRepository> SetupRepositoryReturningDefaultUser()
        {
            var userRepositoryMock = SetupDefaultMockedUserRepository();
            userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(this.defaultUser));

            return userRepositoryMock;
        }

        protected Mock<IUserRepository> SetupRepositoryReturningAdminUser()
        {
            var userRepositoryMock = SetupDefaultMockedUserRepository();
            userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(this.adminUser));

            return userRepositoryMock;
        }

        protected Mock<IUserRepository> SetupRepositoryReturningNull()
        {
            var userRepositoryMock = SetupDefaultMockedUserRepository();
            userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult<User>(null));

            return userRepositoryMock;
        }

        protected UserService SetupMockedUserService(Mock<IUserRepository> userRepositoryMock)
        {
            var validationMock = new Mock<IValidationService>();
            var service = new UserService(userRepositoryMock.Object, validationMock.Object);

            return service;
        }

        protected DbContextOptions<DatabaseContext> SetupDbOptions()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                          .UseInMemoryDatabase(databaseName: "TestDB")
                          .Options;

            return options;
        }

        protected ValidationService SetupMockedValidationService()
        {
            var configMock = new Mock<IConfiguration>();
            var dbContextMock = new Mock<DatabaseContext>(configMock.Object);
            var validation = new ValidationService(dbContextMock.Object);

            return validation;
        }
    }
}
