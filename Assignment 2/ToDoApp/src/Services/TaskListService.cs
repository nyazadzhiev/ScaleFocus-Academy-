using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoAppData;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class TaskListService
    {
        private readonly Database _database;
        private List<TaskList> tasks = new List<TaskList>();
        private int listIDGenerator = 0;

        public TaskListService(Database database)
        {
            _database = database;
        }

        public List<TaskList> GetAllTaskLists(User user)
        {
            return _database.GetTaskLists(user.Id);
        }

        public List<TaskList> GetSharedLists()
        {
            return _database.GetSharedLists();
        }

        public bool CreateTaskList(User user, string title)
        {
            listIDGenerator++;

            return _database.CreateTaskList(new TaskList()
            {
                Id = listIDGenerator,
                Title = title,
                CreatorId = user.Id,
                CreatedAt = DateTime.Now,
                LastEdited = DateTime.Now,
                ModifierId = user.Id
            });
        }

        public TaskList GetTaskList(int id)
        {
            return _database.GetTaskList(id);
        }

        public TaskList GetTaskList(string title)
        {
            return _database.GetTaskList(title);
        }

        public bool EditTaskList(int id, string newTitle)
        {
            TaskList currentList = GetTaskList(id);
            if (currentList == null)
            {
                Console.WriteLine($"There isn't list with id {id}");

                return false;
            }
            else
            {
                _database.EditTaskList(id, newTitle);
                Console.WriteLine("You succesfully edited TaskList");

                return true;
            }
        }

        public bool DeleteTaskList(int id)
        {
            TaskList currentList = GetTaskList(id);
            if (currentList == null)
            {
                Console.WriteLine($"There isn't list with id {id}");

                return false;
            }
            else
            {
                _database.DeleteTaskList(id);
                Console.WriteLine($"You deleted list {currentList.Title}");

                return true;
            }
        }

        public bool ShareTaskList(User user, int listId)
        {
            if(_database.ShareTaskList(user.Id, listId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}


