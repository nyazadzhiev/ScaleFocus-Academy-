using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAppEntities
{
    public class TaskList : Entity
    {
        public string Title { get; set; }
        public List<Task> Tasks { get; set; }

        public TaskList()
        {
            Tasks = new List<Task>();
        }

        public override string ToString()
        {
            return $"ID: {this.Id}\n" +
                $"Title: {this.Title}\n" +
                $"Owner: {this.Creator.FirstName} {this.Creator.LastName}\n" +
                $"Created at: {this.CreatedAt}\n" +
                $"Modifier: {(this.Modifier == null ? "Not edited yet" : $"{this.Modifier.FirstName} {this.Modifier.LastName}")}\n" +
                $"Last Edited: {(this.LastEdited == null ? "Not edited yet" : $"{this.LastEdited}")}\n"; ;
        }
    }
}
