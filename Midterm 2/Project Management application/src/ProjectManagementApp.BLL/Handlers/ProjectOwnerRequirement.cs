using Microsoft.AspNetCore.Authorization;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.BLL.Handlers
{
    public class ProjectOwnerRequirement : IAuthorizationRequirement
    {
        public ProjectOwnerRequirement()
        {
        }
    }
}
