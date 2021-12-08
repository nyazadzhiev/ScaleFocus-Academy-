using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.BLL.Validations
{
    public interface IValidationService
    {
        public void EnsureUserExist(User user);

        public void LoginCheck(User user);

        public void CheckRole(User user);

        public void CheckUsername(string username);

        public void EnsureTeamExist(Team team);

        public void CheckTeamName(string newTeamName);

        public void CanAddToTeam(Team teamFromDB, User userToAdd);

        public void EnsureProjectExist(Project project);

        public void CheckProjectName(string newProjectTitle);

        public void CanAddToProject(Project project, Team team);

        public void EnsureTaskExist(ToDoTask task);
            
        public void CheckTaskName(string title);

        public void CheckProjectAccess(User user, Project project);

        public void EnsureWorkLogExist(WorkLog workLog);

        public void CheckTaskAccess(User currentUser, ToDoTask task);
    }
}
