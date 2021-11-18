using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.DAL.Entities
{
    public class WorkLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ToDoTaskId { get; set; }
        public ToDoTask ToDoTask { get; set; }

        public int WorkedTime { get; set; }
    }
}
