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
        private readonly DatabaseContext _database;
        private Validations validations;

        public TaskListService(DatabaseContext database)
        {
            _database = database;
            validations = new Validations();
        }

        public List<TaskList> GetAllTaskLists(User user)
        {
            return _database.Lists.Where(l => l.CreatorId == user.Id).ToList();
        }

        public List<TaskList> GetSharedLists()
        {
            List<TaskList> lists = new List<TaskList>();

            List<SharedList> sharedLists = _database.SharedLists.Where(l => l.UserId == UserService.CurrentUser.Id).ToList();

            foreach(SharedList sharedList in sharedLists)
            {
                TaskList list = GetTaskList(sharedList.ListId);

                lists.Add(list);
            }

            return lists;
        }

        public bool CreateTaskList(User user, string title)
        {
            TaskList newList = new TaskList()
            {
                Title = title,
                CreatorId = user.Id,
                CreatedAt = DateTime.Now,
                LastEdited = DateTime.Now,
                ModifierId = user.Id
            };

            _database.Lists.Add(newList);

            _database.SaveChanges();

            return newList.Id != 0;
        }

        public TaskList GetTaskList(int id)
        {
            return _database.Lists.FirstOrDefault(l => l.Id == id);
        }

        public TaskList GetTaskList(string title)
        {
            return _database.Lists.FirstOrDefault(l => l.Title == title);
        }

        public bool EditTaskList(int id, string newTitle)
        {
            TaskList currentList = GetTaskList(id);
            bool isValidList = validations.EnsureListExist(currentList);
            if (!isValidList)
            {
                return false;
            }
            else if(currentList.CreatorId != UserService.CurrentUser.Id)
            {
                Console.WriteLine("You don't have permission to do this");

                return false;
            }
            else
            {
                currentList.Title = newTitle;
                currentList.LastEdited = DateTime.Now;
                currentList.ModifierId = UserService.CurrentUser.Id;

                _database.SaveChanges();
                Console.WriteLine("You succesfully edited TaskList");

                return true;
            }
        }

        public bool DeleteTaskList(int id)
        {
            TaskList currentList = GetTaskList(id);
            bool isValidList = validations.EnsureListExist(currentList);
            if (!isValidList)
            {
                return false;
            }
            else if (currentList.CreatorId != UserService.CurrentUser.Id)
            {
                Console.WriteLine("You don't have permission to do this");

                return false;
            }
            else
            {
                _database.Lists.Remove(currentList);
                Console.WriteLine($"You deleted list {currentList.Title}");

                return true;
            }
        }

        public bool ShareTaskList(User user, int listId)
        {
            TaskList toShare = GetTaskList(listId);

            if(toShare.CreatorId != UserService.CurrentUser.Id)
            {
                Console.WriteLine("You don't have permission to do this");

                return false;
            }

            SharedList shared = new SharedList()
            {
                UserId = user.Id,
                ListId = listId
            };

            _database.SharedLists.Add(shared);
            _database.SaveChanges();

            return true;
        }
    }
}


