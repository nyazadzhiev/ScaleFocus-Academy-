using System.Collections.Generic;

namespace ProjectManagementApp.DAL.Entities
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AsigneeId { get; set; }
        public virtual User Asignee { get; set; }
        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public bool IsCompleted { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual List<WorkLog> Worklogs { get; set; }

    }
}