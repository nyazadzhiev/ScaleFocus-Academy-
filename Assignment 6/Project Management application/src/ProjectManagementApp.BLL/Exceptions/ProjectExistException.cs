using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.BLL.Exceptions
{
    public class ProjectExistException : Exception
    {
        public ProjectExistException(string message) : base(message)
        {

        }
    }
}
