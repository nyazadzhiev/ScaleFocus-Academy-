using System;
using System.Collections.Generic;
using System.Text;


namespace ToDoApp.Entities
{
    class User : Person
    {
        public List<TaskList> ToDoList { get; set; }
        public List<TaskList> SharedToDoList { get; set; }
        public User Creator { get; set; }
        public bool IsAdmin { get; set; }

        public User()
        {
            ToDoList = new List<TaskList>();
            SharedToDoList = new List<TaskList>();
        }
    }
}
