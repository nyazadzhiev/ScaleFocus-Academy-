using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoAppData;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class TaskService
    {
        private readonly TaskRepository _database;

        public TaskService(TaskRepository database)
        {
            _database = database;
        }

        public Task GetTask(int id)
        {
            return _database.GetTask(id);
        }

        public Task GetTask(string title)
        {
            return _database.GetTask(title);
        }

        public List<Task> GetTasks(int listId)
        {
            return _database.GetTasks(listId);
        }

        public List<Task> GetAssignedTasks(int userId)
        {
            return _database.GetAssignedTasks(userId);
        }

        public bool AssignTask(User user, int taskId)
        {
            if(_database.AssignTask(user.Id, taskId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CreateTask(User user, TaskList list, string title, string description, bool isComplete)
        {
            return _database.CreateTask(new Task()
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
            });
        }

        public bool CompleteTask(TaskList list, int taskId)
        {
            Task currentTask = _database.GetTask(taskId);
            if (currentTask != null)
            {
                _database.CompleteTask(taskId);

                return true;
            }

            return false;
        }

        public bool EditTask(int taskId, string newTitle, string newDesc, bool newIscomplete)
        {
            Task currentTask = _database.GetTask(taskId);

            if (currentTask == null)
            {
                Console.WriteLine($"There isn't a taks with id {taskId}");

                return false;
            }
            else
            {
                _database.EditTask(taskId, newTitle, newDesc, newIscomplete);

                return true;
            }
        }

        public bool DeteleTask(int taskId)
        {
            Task currentTask = _database.GetTask(taskId);
            if (currentTask == null)
            {
                Console.WriteLine($"There isn't a taks with id {taskId}");

                return false;
            }
            else
            {
                _database.DeleteTask(taskId);
                Console.WriteLine($"You deleted task {currentTask.Title}");

                return true;
            }

        }
    }
}
