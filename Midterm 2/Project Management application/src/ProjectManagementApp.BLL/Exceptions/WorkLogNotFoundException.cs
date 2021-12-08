using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.BLL.Exceptions
{
    public class WorkLogNotFoundException : Exception
    {
        public WorkLogNotFoundException(string message) : base(message)
        {

        }
    }
}
