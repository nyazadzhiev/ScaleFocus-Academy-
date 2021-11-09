using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Data.SqlClient;
using System.Data.Entity;
using ToDoAppEntities;
using Task = ToDoAppEntities.Task;
using Microsoft.EntityFrameworkCore;

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
        public IDbSet<SharedList> SharedLists { get; set; }
        public IDbSet<AssignedTask> AssignedTasks { get; set; }

        protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            SetupUserConfiguration(modelBuilder);
            SetupTaskListConfiguration(modelBuilder);
            SetupTaskConfiguration(modelBuilder);
            SetupSharedListConfiguretion(modelBuilder);
            SetupAssignedTaskConfiguretion(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SetupTaskConfiguration(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().HasKey(t => t.Id);
            modelBuilder.Entity<Task>().Property(t => t.Title).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Task>().Property(t => t.Description).IsRequired().HasMaxLength(300);
            modelBuilder.Entity<Task>().HasRequired(t => t.ToDoList).WithMany(l => l.Tasks).HasForeignKey(t => t.ListId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Task>().HasRequired(t => t.Creator).WithMany().HasForeignKey(u => u.CreatorId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Task>().HasRequired(t => t.Modifier).WithMany().HasForeignKey(u => u.ModifierId).WillCascadeOnDelete(false);
        }

        private static void SetupTaskListConfiguration(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskList>().HasKey(l => l.Id);
            modelBuilder.Entity<TaskList>().Property(l => l.Title).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<TaskList>().HasRequired(l => l.Creator).WithMany().HasForeignKey(u => u.CreatorId).WillCascadeOnDelete(false);
            modelBuilder.Entity<TaskList>().HasRequired(l => l.Modifier).WithMany().HasForeignKey(u => u.ModifierId).WillCascadeOnDelete(false);
        }

        private static void SetupUserConfiguration(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().HasOptional(u => u.Creator).WithMany().HasForeignKey(u => u.CreatorId).WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasOptional(u => u.Modifier).WithMany().HasForeignKey(u => u.ModifierId).WillCascadeOnDelete(false);
        }

        private static void SetupSharedListConfiguretion(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SharedList>().HasKey(l => l.Id);
            modelBuilder.Entity<SharedList>().HasRequired(l => l.User).WithMany().HasForeignKey(s => s.UserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<SharedList>().HasRequired(l => l.List).WithMany().HasForeignKey(s => s.ListId).WillCascadeOnDelete(false);
        }

        private static void SetupAssignedTaskConfiguretion(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssignedTask>().HasKey(t => t.Id);
            modelBuilder.Entity<AssignedTask>().HasRequired(t => t.User).WithMany().HasForeignKey(t => t.UserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<AssignedTask>().HasRequired(t => t.Task).WithMany().HasForeignKey(t => t.TaskId).WillCascadeOnDelete(false);
        }
    }
}
