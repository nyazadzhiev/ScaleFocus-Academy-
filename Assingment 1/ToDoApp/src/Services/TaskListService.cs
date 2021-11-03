using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class TaskListService
    {
        private List<TaskList> tasks = new List<TaskList>();
        private int listIDGenerator = 0;

        public List<TaskList> GetAllTaskLists()
        {
            return tasks;
        }

        public TaskList CreateTaskList(User user, string title)
        {
            listIDGenerator++;

            TaskList newTaskList = new TaskList()
            {
                Id = listIDGenerator,
                Title = title,
                Creator = user,
                CreatedAt = DateTime.Now,
                LastEdited = null,
                Modifier = null
            };

            user.ToDoList.Add(newTaskList);
            tasks.Add(newTaskList);

            return newTaskList;

        }

        public TaskList GetTaskList(User user, int id)
        {
            return user.ToDoList.FirstOrDefault(t => t.Id == id);
        }

        public bool EditTaskList(int id, string newTitle)
        {
            TaskList currentList = GetTaskList(UserService.CurrentUser, id);
            if (currentList == null)
            {
                Console.WriteLine($"There isn't list with id {id}");

                return false;
            }
            else
            {
                currentList.Title = newTitle;
                currentList.LastEdited = DateTime.Now;
                currentList.Modifier = UserService.CurrentUser;
                Console.WriteLine("You succesfully edited TaskList");

                return true;
            }
        }

        public bool DeleteTaskList(int id)
        {
            TaskList currentList = GetTaskList(UserService.CurrentUser, id);
            if(currentList == null)
            {
                Console.WriteLine($"There isn't list with id {id}");

                return false;
            }
            else
            {
                UserService.CurrentUser.ToDoList.Remove(currentList);
                Console.WriteLine($"You deleted list {currentList.Title}");

                return true;
            }
        }
    }
}

