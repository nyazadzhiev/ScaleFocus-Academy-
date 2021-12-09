using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Contracts
{
    public interface ITaskService
    {
        public Task<bool> CreateTask(string title, string description, bool isCompleted, int projectId, User currentUser, string userId);

        public Task<ToDoTask> GetTask(int id);

        public Task<ToDoTask> GetTask(string title);

        public Task<ToDoTask> GetTask(int taskId, int projectId, User user);

        public Task<List<ToDoTask>> GetAll(int projectId, User currentUser);

        public Task<bool> DeleteTask(int taskId, int projectId, User user);

        public Task<bool> EditTask(int taskId, int projectId, User user, string newTaskTitle, string newDesc, bool newStatus);

        public Task<bool> ChangeStatus(int taskId, int projectId, User user);
    }
}
