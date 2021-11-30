using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DAL.Repositories
{
    public interface ITaskRepository
    {
        public Task AddTaskAsync(ToDoTask task);

        public Task SaveChangesAsync();

        public Task<ToDoTask> GetTaskAsync(int id);

        public Task<ToDoTask> GetTaskAsync(string title);

        public Task<List<ToDoTask>> GetTasksAsync(int projectId);

        public void DeleteTask(ToDoTask task);
    }
}
