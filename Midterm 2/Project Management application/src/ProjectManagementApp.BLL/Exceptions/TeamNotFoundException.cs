using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.BLL.Exceptions
{
    public class TeamNotFoundException : Exception
    {
        public TeamNotFoundException(string message) : base(message)
        {

        }
    }
}
