using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.WEB.Models.Responses
{
    public class WorkLogResponseModel
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string UserId { get; set; }

        public int WorkedHours { get; set; }
    }
}
