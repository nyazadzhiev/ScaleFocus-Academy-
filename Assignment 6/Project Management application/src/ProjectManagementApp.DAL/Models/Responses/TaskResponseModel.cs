using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.DAL.Models.Responses
{
    public class TaskResponseModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public int OwnerId { get; set; }

        public int AsigneeId { get; set; }

        public int ProjectId { get; set; }

        public int TotalWorkedHours { get; set; }
    }
}
