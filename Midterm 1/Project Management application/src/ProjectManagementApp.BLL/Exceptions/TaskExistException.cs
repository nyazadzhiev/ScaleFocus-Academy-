using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.BLL.Exceptions
{
    public class TaskExistException : Exception
    {
        public TaskExistException(string message) : base(message)
        {

        }
    }
}
