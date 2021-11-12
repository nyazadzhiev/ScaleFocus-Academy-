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
using ToDoTask = ToDoAppEntities.ToDoTask;
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
        public IDbSet<ToDoTask> Tasks { get; set; }

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
            modelBuilder.Entity<ToDoTask>().HasKey(t => t.Id);
            modelBuilder.Entity<ToDoTask>().Property(t => t.Title).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<ToDoTask>().Property(t => t.Description).IsRequired().HasMaxLength(300);
            modelBuilder.Entity<ToDoTask>().HasRequired(t => t.ToDoList).WithMany(l => l.Tasks).HasForeignKey(t => t.ListId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ToDoTask>().HasRequired(t => t.Creator).WithMany().HasForeignKey(u => u.CreatorId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ToDoTask>().HasRequired(t => t.Modifier).WithMany().HasForeignKey(u => u.ModifierId).WillCascadeOnDelete(false);
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
            modelBuilder.Entity<User>()
                        .HasMany<TaskList>(u => u.SharedLists)
                        .WithMany(l => l.SharedUsers)
                        .Map(ul =>
                        {
                            ul.MapLeftKey("UserId");
                            ul.MapRightKey("ListId");
                            ul.ToTable("SharedLists");
                        });
        }

        private static void SetupAssignedTaskConfiguretion(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany<ToDoTask>(u => u.AssignedTasks)
            .WithMany(t => t.SharedUsers)
            .Map(ut =>
            {
                ut.MapLeftKey("UserId");
                ut.MapRightKey("TaskId");
                ut.ToTable("AssignedLists");
            });
        }
    }
}
