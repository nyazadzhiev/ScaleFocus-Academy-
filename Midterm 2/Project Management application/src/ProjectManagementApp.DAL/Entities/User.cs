using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.DAL.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<Team> Teams { get; set; }
        public virtual List<ToDoTask> ToDoTasks { get; set; }
        public virtual List<WorkLog> WorkLogs { get; set; }

        public User()
        {
            this.Teams = new List<Team>();
            this.ToDoTasks = new List<ToDoTask>();
            this.WorkLogs = new List<WorkLog>();
        }
    }
}
