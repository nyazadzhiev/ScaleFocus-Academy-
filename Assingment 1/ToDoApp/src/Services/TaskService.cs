using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class TaskService
    {
        private int taskIDGenerator = 0;

        public Task GetTask(TaskList list, int id)
        {
            return list.Tasks.FirstOrDefault(t => t.Id == id);
        }

        public void CreateTask(TaskList list, User creator, string title, string description, bool isComplete)
        {
            taskIDGenerator++;

            Task newTask = new Task()
            {
                Creator = creator,
                Title = title,
                Description = description,
                IsComplete = isComplete,
                ToDoList = list,
                CreatedAt = DateTime.Now,
                LastEdited = null,
                Id = taskIDGenerator
            };

            list.Tasks.Add(newTask);
        }

        public bool CompleteTask(TaskList list, int taskId)
        {
            Task currentTask = list.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (currentTask != null)
            {
                currentTask.IsComplete = true;

                return true;
            }

            return false;
        }

        public bool EditTask(TaskList list, int taskId, string newTitle, string newDesc, bool newIscomplete)
        {
            Task currentTask = list.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (currentTask == null)
            {
                Console.WriteLine($"There isn't a taks with id {taskId}");

                return false;
            }
            else
            {
                currentTask.Title = newTitle;
                currentTask.Description = newDesc;
                currentTask.IsComplete = newIscomplete;
                currentTask.LastEdited = DateTime.Now;

                return true;
            }
        }

        public bool DeteleTask(TaskList list, int taskId)
        {
            Task currentTask = list.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (currentTask == null)
            {
                Console.WriteLine($"There isn't a taks with id {taskId}");

                return false;
            }
            else
            {
                list.Tasks.Remove(currentTask);
                Console.WriteLine($"You deleted task {currentTask.Title}");

                return true;
            }

        }
    }
}

