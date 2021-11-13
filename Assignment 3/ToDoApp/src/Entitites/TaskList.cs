using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAppEntities
{
    public class TaskList : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual List<ToDoTask> Tasks { get; set; }
        public virtual List<User> SharedUsers { get; set; }

        public TaskList()
        {
            this.Tasks = new List<ToDoTask>();
            this.SharedUsers = new List<User>();
        }

        public override string ToString()
        {
            return $"ID: {this.Id}\n" +
                $"Title: {this.Title}\n" +
               $"Creator: {this.Creator.FirstName} {this.Creator.LastName}\n" +
                $"Created at: {this.CreatedAt}\n" +
                $"Modifier: {this.Modifier.FirstName} {this.Modifier.LastName}\n" +
                $"Last Edited: {this.LastEdited}\n";
        }
    }
}
