using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.BLL.Contracts
{
    public interface IValidationService
    {
        void EnsureUserExist(User user);

        void LoginCheck(User user);

        void CheckRole(string role);

        void CheckUsername(string username);

        void EnsureTeamExist(Team team);

        void CheckTeamName(string newTeamName);

        void CanAddToTeam(Team teamFromDB, User userToAdd);

        void EnsureProjectExist(Project project);

        void CheckProjectName(string newProjectTitle);

        void CanAddToProject(Project project, Team team);

        void EnsureTaskExist(ToDoTask task);

        void CheckTaskName(string title);

        void CheckProjectAccess(User user, Project project);

        void EnsureWorkLogExist(WorkLog workLog);

        void CheckTaskAccess(User currentUser, ToDoTask task);
    }
}
