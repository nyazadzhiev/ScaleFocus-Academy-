using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.WEB.Models.Responses
{
    public class TaskResponseModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public string OwnerId { get; set; }

        public string AsigneeId { get; set; }

        public int ProjectId { get; set; }

        public int TotalWorkedHours { get; set; }
    }
}
