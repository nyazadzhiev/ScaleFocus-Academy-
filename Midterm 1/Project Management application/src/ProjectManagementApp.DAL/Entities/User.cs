﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public List<Team> Teams { get; set; }
        public List<ToDoTask> Tasks { get; set; }
    }
}
