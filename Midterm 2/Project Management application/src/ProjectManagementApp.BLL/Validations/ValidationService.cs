using Common;
using ProjectManagementApp.BLL.Contracts;
using ProjectManagementApp.BLL.Exceptions;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManagementApp.BLL.Validations
{
    public class ValidationService : IValidationService
    {
        private readonly DatabaseContext database;

        public ValidationService(DatabaseContext _database)
        {
            database = _database;
        }

        public void EnsureUserExist(User user)
        {
            if (user == null)
            {
                throw new UserNotFoundException(String.Format(Constants.NotFound, "User"));
            }
        }

        public void LoginCheck(User user)
        {
            if (user == null)
            {
                throw new UnauthorizedUserException(Constants.LoginFailed);
            }
        }

        /*public void CheckRole(User user)
        {
            if (!user.IsAdmin)
            {
                throw new UnauthorizedUserException(Constants.Unauthorized);
            }
        }*/


        public void CheckUsername(string username)
        {
            if(database.Users.Any(u => u.UserName == username))
            {
                throw new UserExistException(String.Format(Constants.Exist, "User"));
            }
        }

        public void EnsureTeamExist(Team team)
        {
            if (team == null)
            {
                throw new TeamNotFoundException(String.Format(Constants.NotFound, "Team"));
            }
        }

        public void CheckTeamName(string newTeamName)
        {
            if(database.Teams.Any(t => t.Name == newTeamName))
            {
                throw new TeamExistException(String.Format(Constants.Exist, "team"));
            }
        }

        public void CanAddToTeam(Team teamFromDB, User userToAdd)
        {
            if(teamFromDB.Users.Any(u => u.UserName == userToAdd.UserName))
            {
                throw new UserExistException(Constants.UserAlreadyAssigned);
            }
        }

        public void EnsureProjectExist(Project project)
        {
            if (project == null)
            {
                throw new ProjectNotFoundException(String.Format(Constants.NotFound, "Project"));
            }
        }

        public void CheckProjectName(string newProjectTitle)
        {
            if(database.Projects.Any(p => p.Title == newProjectTitle))
            {
                throw new ProjectExistException(String.Format(Constants.Exist, "Project"));
            }
        }

        public void CanAddToProject(Project project, Team team)
        {
            if(project.Teams.Any(t => t.Id == team.Id))
            {
                throw new TeamExistException(String.Format(Constants.Exist, "Team"));
            }
        }

        public void EnsureTaskExist(ToDoTask task)
        {
            if (task == null)
            {
                throw new TaskNotFoundException(String.Format(Constants.NotFound, "Task"));
            }
        }

        public void CheckTaskName(string title)
        {
            if(database.ToDoTasks.Any(t => t.Title == title))
            {
                throw new TaskExistException(String.Format(Constants.Exist, "Task"));
            }
        }

        public void CheckProjectAccess(User user, Project project)
        {
            if(!project.OwnerId.Equals(user.Id) && !project.Teams.Any(t => t.Users.Any(u => u.UserName == user.UserName)))
            {
                throw new UnauthorizedUserException(Constants.Unauthorized);
            }
        }

        public void EnsureWorkLogExist(WorkLog workLog)
        {
            if(workLog == null)
            {
                throw new WorkLogNotFoundException(String.Format(Constants.NotFound, "WorkLog"));
            }
        }

        public void CheckTaskAccess(User currentUser, ToDoTask task)
        {
            if(!task.AsigneeId.Equals(currentUser.Id))
            {
                throw new UnauthorizedUserException(Constants.Unauthorized);
            }
        }
    }
}
