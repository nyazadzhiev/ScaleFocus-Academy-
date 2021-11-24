using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.BLL.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(string message) : base(message)
        {

        }
    }
}
