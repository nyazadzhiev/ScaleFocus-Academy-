using ProjectManagementApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Contracts
{
    public interface ITaskService
    {
        Task<bool> CreateTask(string title, string description, bool isCompleted, int projectId, User currentUser, string userId);

        Task<ToDoTask> GetTaskById(int id);

        Task<ToDoTask> GetTaskByTitle(string title);

        Task<ToDoTask> GetTask(int taskId, int projectId, User user);

        Task<List<ToDoTask>> GetAll(int projectId, User currentUser);

        Task<bool> DeleteTask(int taskId, int projectId, User user);

        Task<bool> EditTask(int taskId, int projectId, User user, string newTaskTitle, string newDesc, bool newStatus);

        Task<bool> ChangeStatus(int taskId, int projectId, User user);

        Task<bool> Reassign(int taskId, int projectId, string userId, User currentUser);
    }
}
