using System;
using System.Collections.Generic;
using System.Text;


namespace ToDoAppEntities
{
    public class User : Person
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; }

        public virtual List<TaskList> SharedLists { get; set;  }
        public virtual List<ToDoTask> AssignedTasks { get; set; }

        public User()
        {
            this.SharedLists = new List<TaskList>();
            this.AssignedTasks = new List<ToDoTask>();
        }

        public override string ToString()
        {
            return $"Name {this.FirstName} {this.LastName}\n" +
                $"Role: {(this.IsAdmin ? "Admin" : "User")}\n" +
               $"Creator: {this.Creator.FirstName} {this.Creator.LastName}\n" +
                $"Created at: {this.CreatedAt}\n" +
                $"Modifier: {this.Modifier.FirstName} {this.Modifier.LastName}\n" +
                $"Last Edited: {this.LastEdited}\n";
        }
    }
}