using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Data.SqlClient;
using System.Data;
using ToDoAppEntities;
using Task = ToDoAppEntities.Task;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace ToDoAppData
{
    public class DatabaseContext : System.Data.Entity.DbContext
    {
        public DatabaseContext(string connectionString) : base(connectionString)
        {

        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<TaskList> Lists { get; set; }
        public IDbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasRequired(u => u.Creator).WithMany();
            modelBuilder.Entity<User>().HasRequired(u => u.Modifier).WithMany();
            modelBuilder.Entity<TaskList>().HasRequired(t => t.Creator).WithMany();
            modelBuilder.Entity<TaskList>().HasRequired(t => t.Modifier).WithMany();
            modelBuilder.Entity<Task>().HasRequired(t => t.ToDoList).WithMany(l => l.Tasks);
            modelBuilder.Entity<Task>().HasRequired(t => t.Creator).WithMany();
            modelBuilder.Entity<Task>().HasRequired(t => t.Modifier).WithMany();
        }

    }
}
