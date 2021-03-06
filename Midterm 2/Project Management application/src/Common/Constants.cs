using System;

namespace Common
{
    public static class Constants
    {
        public const string NotFound = "{0} not found";

        public const string Unauthorized = "You don't have permission";

        public const string FailedOperation = "Operation Failed";

        public const string Created = "{0} was created";

        public const string Deleted = "{0} was deleted";

        public const string Exist = "{0} already exist";

        public const string UserAddedToTeam = "User was added to team";

        public const string UserInTeam = "User already is in the team";

        public const string UserRemovedFromTeam = "User removed from team";

        public const string UserAlreadyAssigned = "User is already assigned";

        public const string TeamAddedToProject = "Team was added to project";

        public const string TeamRemovedFromProject = "Team was removed from project";

        public const string StatusChanged = "Task status was changed";

        public const string TaskReassigned = "Task assigne was changed";

        public const string LoginFailed = "Login failed";

        public const string Admin = "Admin";

        public const string Manager = "Manager";

        public const string User = "User";

        public const string InvalidInput = "You entered invalid value";

        public static string[] Roles = new string[] { Admin.ToUpper(), Manager.ToUpper(), User.ToUpper() };
    }
}
