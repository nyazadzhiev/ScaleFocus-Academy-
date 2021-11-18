using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public List<ToDoTask> ToDoTasks { get; set; }

        public User()
        {
            this.ToDoTasks = new List<ToDoTask>();
        }
    }
}
