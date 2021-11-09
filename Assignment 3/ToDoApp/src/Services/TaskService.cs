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
        private readonly DatabaseContext _database;
        private Validations validations;

        public TaskService(DatabaseContext database)
        {
            _database = database;
            validations = new Validations();
        }

        public Task GetTask(int id)
        {
            return _database.Tasks.FirstOrDefault(t => t.Id == id);
        }

        public Task GetTask(string title)
        {
            return _database.Tasks.FirstOrDefault(t => t.Title == title);
        }

        public List<Task> GetTasks(int listId)
        {
            return _database.Tasks.Where(t => t.ListId == listId).ToList();
        }

        public List<Task> GetAssignedTasks()
        {
            List<Task> tasks = new List<Task>();

            List<AssignedTask> assignedTasks = _database.AssignedTasks.Where(t => t.UserId == UserService.CurrentUser.Id).ToList();

            foreach(AssignedTask assignedTask in assignedTasks)
            {
                Task task = GetTask(assignedTask.TaskId);

                tasks.Add(task);
            }

            return tasks;
        }

        public bool AssignTask(User user, int taskId)
        {
            Task toAssign = GetTask(taskId);

            if (toAssign.CreatorId != UserService.CurrentUser.Id)
            {
                Console.WriteLine("You don't have permission to do this");

                return false;
            }

            AssignedTask assignedTask = new AssignedTask()
            {
                UserId = user.Id,
                TaskId = taskId
            };

            _database.AssignedTasks.Add(assignedTask);
            _database.SaveChanges();

            return true;
        }

        public bool CreateTask(User user, TaskList list, string title, string description, bool isComplete)
        {
            Task newTask = new Task()
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
            _database.SaveChanges();

            return newTask.Id != 0;
        }

        public bool CompleteTask(TaskList list, int taskId)
        {
            Task currentTask = GetTask(taskId);
            bool isValidTask = validations.EnsureTaskExist(currentTask);
            if (isValidTask)
            {
                currentTask.IsComplete = true;
                currentTask.LastEdited = DateTime.Now;
                currentTask.ModifierId = UserService.CurrentUser.Id;

                _database.SaveChanges();

                return true;
            }

            return false;
        }

        public bool EditTask(int taskId, string newTitle, string newDesc, bool newIsComplete)
        {
            Task currentTask = GetTask(taskId);

            bool isValidTask = validations.EnsureTaskExist(currentTask);
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

                _database.SaveChanges();

                return true;
            }
        }

        public bool DeteleTask(int taskId)
        {
            Task currentTask = GetTask(taskId);
            bool isValidTask = validations.EnsureTaskExist(currentTask);
            if (!isValidTask)
            {
                return false;
            }
            else
            {
                _database.Tasks.Remove(currentTask);
                Console.WriteLine($"You deleted task {currentTask.Title}");

                return true;
            }

        }
    }
}
