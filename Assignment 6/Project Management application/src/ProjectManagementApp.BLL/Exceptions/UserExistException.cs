using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.BLL.Exceptions
{
    public class UserExistException : Exception
    {
        public UserExistException(string message) : base(message)
        {
        }
    }
}
