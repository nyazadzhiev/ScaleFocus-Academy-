using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoAppData;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class TaskService
    {
        private readonly DatabaseContext _database;
        private Validations validations;

        public TaskService(DatabaseContext database)
        {
            _database = database;
            validations = new Validations();
        }

        public ToDoTask GetTask(int id)
        {
            return _database.Tasks.FirstOrDefault(t => t.Id == id);
        }

        public ToDoTask GetTask(string title)
        {
            return _database.Tasks.FirstOrDefault(t => t.Title == title);
        }

        public List<ToDoTask> GetTasks(int listId)
        {
            return _database.Tasks.Where(t => t.ListId == listId).ToList();
        }

        public List<ToDoTask> GetAssignedTasks()
        {
            return UserService.CurrentUser.AssignedTasks;
        }

        public async Task<bool> AssignTask(User user, int taskId)
        {
            ToDoTask toAssign = GetTask(taskId);

            if (toAssign.CreatorId != UserService.CurrentUser.Id)
            {
                throw new NotSupportedException();
            }

            user.AssignedTasks.Add(toAssign);
            await _database.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateTask(User user, TaskList list, string title, string description, bool isComplete)
        {
            ToDoTask newTask = new ToDoTask()
            {
                Creator = user,
                Title = title,
                Description = description,
                IsComplete = isComplete,
                ListId = list.Id,
                CreatedAt = DateTime.Now,
                LastEdited = DateTime.Now,
                ModifierId = user.Id,
                CreatorId = user.Id
            };

            _database.Tasks.Add(newTask);
            await _database.SaveChangesAsync();

            return newTask.Id != 0;
        }

        public async Task<bool> CompleteTask(TaskList list, int taskId)
        {
            ToDoTask currentTask = GetTask (taskId);
            bool isValidTask = validations.EnsureTaskExist(currentTask);

            validations.CheckAccessToTask(currentTask);

            if (isValidTask)
            {
                currentTask.IsComplete = true;
                currentTask.LastEdited = DateTime.Now;
                currentTask.ModifierId = UserService.CurrentUser.Id;

                await _database.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> EditTask(int taskId, string newTitle, string newDesc, bool newIsComplete)
        {
            ToDoTask currentTask = GetTask (taskId);

            bool isValidTask = validations.EnsureTaskExist(currentTask);

            validations.CheckAccessToTask(currentTask);

            if (!isValidTask)
            {
                return false;
            }
            else
            {
                currentTask.Title = newTitle;
                currentTask.Description = newDesc;
                currentTask.IsComplete = newIsComplete;
                currentTask.LastEdited = DateTime.Now;
                currentTask.ModifierId = UserService.CurrentUser.Id;

                await _database.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> DeteleTask(int taskId)
        {
            ToDoTask currentTask = GetTask (taskId);
            bool isValidTask = validations.EnsureTaskExist(currentTask);

            validations.CheckAccessToTask(currentTask);

            if (!isValidTask)
            {
                return false;
            }
            else
            {
                _database.Tasks.Remove(currentTask);
                await _database.SaveChangesAsync();

                Console.WriteLine($"You deleted task {currentTask.Title}");

                return true;
            }

        }
    }
}
