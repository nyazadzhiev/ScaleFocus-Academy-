using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoApp.Entities
{
    public class TaskList : Entity
    {
        public string Title { get; set; }
        public List<Task> Tasks { get; set; }
        public User Owner { get; set; }

        public TaskList()
        {
            Tasks = new List<Task>();
        }

        public override string ToString()
        {
            return $"ID: {this.Id}\n" +
                $"Title: {this.Title}\n" +
                $"Owner: {this.Owner.FirstName} {this.Owner.LastName}";
        }
    }
}
