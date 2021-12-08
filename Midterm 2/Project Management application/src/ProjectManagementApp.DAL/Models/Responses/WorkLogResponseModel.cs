using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.DAL.Models.Responses
{
    public class WorkLogResponseModel
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public int UserId { get; set; }

        public int WorkedHours { get; set; }
    }
}
