using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.DAL.Entities
{
    public class WorkLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ToDoTaskId { get; set; }
        public virtual ToDoTask ToDoTask { get; set; }

        public int WorkedTime { get; set; }
    }
}
