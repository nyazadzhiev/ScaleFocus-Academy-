using System;
using System.Collections.Generic;
using System.Text;


namespace ToDoAppEntities
{
    public class User : Person
    {
        public List<TaskList> ToDoList { get; set; }
        public bool IsAdmin { get; set; }

        public User()
        {
            ToDoList = new List<TaskList>();
        }

        public override string ToString()
        {
            return $"Name {this.FirstName} {this.LastName}\n" +
                $"Role: {(this.IsAdmin ? "Admin" : "User")}\n" +
                $"Created at: {this.CreatedAt}\n" +
                $"Last Edited: {this.LastEdited}\n";
        }
    }
}