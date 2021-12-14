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

        public Task<ToDoTask> GetTaskByIdAsync(int id);

        public Task<ToDoTask> GetTaskByTitleAsync(string title);

        public Task<List<ToDoTask>> GetTasksAsync(int projectId);

        public void DeleteTask(ToDoTask task);
    }
}
