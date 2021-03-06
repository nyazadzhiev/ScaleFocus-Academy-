using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAppEntities
{
    public class ToDoTask : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
        public int ListId { get; set; }
        public virtual TaskList ToDoList { get; set; }
        public virtual List<User> SharedUsers { get; set; }

        public ToDoTask()
        {
            this.SharedUsers = new List<User>();
        }

        public override string ToString()
        {
            return $"Id {this.Id} \n" +
                $"Title: {this.Title} \n" +
                $"Description: {this.Description} \n" +
                $"Is completed: {this.IsComplete} \n" +
                $"Creator: {this.Creator.FirstName} {this.Creator.LastName}\n" +
                $"Created at: {this.CreatedAt}\n" +
                $"Modifier: {this.Modifier.FirstName} {this.Modifier.LastName}\n" +
                $"Last Edited: {this.LastEdited}\n";
        }
    }
}
