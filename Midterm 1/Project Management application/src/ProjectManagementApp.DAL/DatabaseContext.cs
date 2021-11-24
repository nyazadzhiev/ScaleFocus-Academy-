using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.DAL
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration _config;

        public DatabaseContext(IConfiguration config) : base()
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ToDoTask> ToDoTasks { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer(_config.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupUserConfiguration(modelBuilder);
            SetupTeamConfiguration(modelBuilder);
            SetupProjectConfiguration(modelBuilder);
            SetupTaskConfiguration(modelBuilder);
            SetupWorkLogConfiguration(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SetupUserConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().HasMany<Team>(u => u.Teams).WithMany(t => t.Users);
            modelBuilder.Entity<User>().HasMany<ToDoTask>(u => u.ToDoTasks).WithOne(t => t.Asignee);
        }

        private static void SetupTeamConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().HasKey(t => t.Id);
            modelBuilder.Entity<Team>().Property(t => t.Name).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Team>().HasMany<User>(t => t.Users).WithMany(u => u.Teams);
        }

        private static void SetupProjectConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasKey(p => p.Id);
            modelBuilder.Entity<Project>().Property(p => p.Title).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Project>().HasOne(p => p.Owner).WithMany().HasForeignKey(p => p.OwnerId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Project>().HasMany(p => p.Teams).WithMany(t => t.Projects);
        }

        private static void SetupTaskConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoTask>().HasKey(t => t.Id);
            modelBuilder.Entity<ToDoTask>().Property(t => t.Title).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<ToDoTask>().Property(t => t.Description).IsRequired().HasMaxLength(300);
            modelBuilder.Entity<ToDoTask>().HasOne(t => t.Asignee).WithMany().HasForeignKey(t => t.AsigneeId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ToDoTask>().HasOne(t => t.Owner).WithMany().HasForeignKey(t => t.OwnerId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ToDoTask>().HasOne(t => t.Project).WithMany().HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Cascade);
        }

        private static void SetupWorkLogConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkLog>().HasKey(w => w.Id);
            modelBuilder.Entity<WorkLog>().HasOne(w => w.User).WithMany(u => u.WorkLogs).HasForeignKey(w => w.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<WorkLog>().HasOne(w => w.ToDoTask).WithMany().HasForeignKey(w => w.ToDoTaskId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
