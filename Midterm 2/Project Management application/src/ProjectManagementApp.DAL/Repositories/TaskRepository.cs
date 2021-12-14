using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DatabaseContext database;

        public TaskRepository(DatabaseContext context)
        {
            database = context;
        }

        public async Task AddTaskAsync(ToDoTask task)
        {
            await database.ToDoTasks.AddAsync(task);
        }

        public async Task SaveChangesAsync()
        {
            await database.SaveChangesAsync();
        }

        public async Task<ToDoTask> GetTaskByIdAsync(int id)
        {
            return await database.ToDoTasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ToDoTask> GetTaskByTitleAsync(string title)
        {
            return await database.ToDoTasks.FirstOrDefaultAsync(t => t.Title == title);
        }

        public async Task<List<ToDoTask>> GetTasksAsync(int projectId)
        {
            return await database.ToDoTasks.Where(t => t.ProjectId == projectId).ToListAsync();
        }

        public void DeleteTask(ToDoTask task)
        {
            database.ToDoTasks.Remove(task);
        }
    }
}
