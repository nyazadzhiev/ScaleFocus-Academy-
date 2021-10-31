using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAppEntities
{
    public class Task : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
        public User Creator { get; set; }
        public TaskList ToDoList { get; set; }

        public override string ToString()
        {
            return $"Id {this.Id} \n" +
                $"Title: {this.Title} \n" +
                $"Description: {this.Description} \n" +
                $"Is completed: {this.IsComplete} \n" +
                $"User: {this.Creator.FirstName} {this.Creator.LastName}\n" +
                $"Created at {this.CreatedAt}\n" +
                $"Modifier: {(this.Modifier == null ? "Not edited yet" : $"{this.Modifier.FirstName} {this.Modifier.LastName}")}\n" +
                $"Last Edited: {(this.LastEdited == null ? "Not edited yet" : $"{this.LastEdited}")}\n";
        }
    }
}
