using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAppEntities
{
    public class TaskList : Entity
    {
        public int Id { get; set; }
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
                $"Created at: {this.CreatedAt}\n" +
                $"Last Edited: {(this.LastEdited == default(DateTime) ? "Not edited yet" : $"{this.LastEdited}")}\n"; ;
        }
    }
}
