using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAppEntities
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
                $"Owner: {this.Owner.FirstName} {this.Owner.LastName}\n" +
                $"Created at: {this.CreatedAt}\n" +
                $"Last Edited: {(this.LastEdited == null ? "Not edited yet" : $"{this.LastEdited}")}\n"; ;
        }
    }
}
