using ProjectManagementApp.BLL.Exceptions;
using ProjectManagementApp.DAL;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManagementApp.BLL.Validations
{
    public class Validation
    {
        private readonly DatabaseContext database;

        public Validation(DatabaseContext _database)
        {
            database = _database;
        }

        public bool EnsureUserExist(User user)
        {
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            else
            {
                return true;
            }
        }

        public void CheckRole(User user)
        {
            if (!user.IsAdmin)
            {
                throw new UnauthorizedUserException();
            }
        }

        public bool CheckUsername(string username)
        {
            return database.Users.Any(u => u.Username == username);
        }

        public bool EnsureTeamExist(Team team)
        {
            if (team == null)
            {
                throw new TeamNotFoundException();
            }
            else
            {
                return true;
            }
        }

        public bool CheckTeamName(string newTeamName)
        {
            return database.Teams.Any(t => t.Name == newTeamName);
        }

        public bool CanAddToTeam(Team teamFromDB, User userToAdd)
        {
            return teamFromDB.Users.Any(u => u.Username == userToAdd.Username);
        }

        public bool EnsureProjectExist(Project project)
        {
            if (project == null)
            {
                throw new ProjectNotFoundException();
            }
            else
            {
                return true;
            }
        }

        public bool CheckProjectName(string newProjectTitle)
        {
            return database.Projects.Any(p => p.Title == newProjectTitle);
        }

        public bool CanAddToProject(Project project, Team team)
        {
            return project.Teams.Any(t => t.Id == team.Id);
        }

        public bool EnsureTaskExist(ToDoTask task)
        {
            if (task == null)
            {
                throw new ProjectNotFoundException();
            }
            else
            {
                return true;
            }
        }

        public bool CheckTaskName(string title)
        {
            return database.ToDoTasks.Any(t => t.Title == title);
        }
    }
}
