using System;
using System.Collections.Generic;
using System.Text;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class TaskListCommand
    {
        UserService _userService;
        TaskListService _listService;

        public TaskListCommand(UserService userService, TaskListService listService)
        {
            _userService = userService;
            _listService = listService;
        }

        public void PromptCreateTaskList()
        {
            Console.WriteLine("Enter title");
            string title = Console.ReadLine();
            if (String.IsNullOrEmpty(title))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }

            _listService.CreateTaskList(UserService.CurrentUser, title);

            Console.WriteLine($"You created a tasklist {title}");
        }

        public void PromptEditTaskList()
        {
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            Console.WriteLine("Enter id");
            string _id = Console.ReadLine();
            int id;
            if (int.TryParse(_id, out id))
            {
            }
            else
            {
                Console.WriteLine("Invalid input");

                return;
            }

            TaskList list = _listService.GetTaskList(id);

            if (list == null)
            {
                Console.WriteLine($"There isn't a list with id: {id}.");
                return;
            }

            Console.WriteLine($"You want to edit list {list.Title}");

            Console.WriteLine("Enter new title");
            string newTitle = Console.ReadLine();

            _listService.EditTaskList(id, newTitle);
        }

        public void PromptDeleteTaskList()
        {
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            Console.WriteLine("Enter id");
            string _id = Console.ReadLine();
            int id;
            if (int.TryParse(_id, out id))
            {
            }
            else
            {
                Console.WriteLine("Invalid input");

                return;
            }

            _listService.DeleteTaskList(id);
        }

        public void PromptShareTaskList()
        {
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            Console.WriteLine("Enter receiver username");
            string username = Console.ReadLine();

            if (username == UserService.CurrentUser.Username)
            {
                Console.WriteLine("You can't share with yourself");

                return;
            }

            User receiver = _userService.GetUser(username);

            if (receiver == null)
            {
                Console.WriteLine($"There isn't user with username {username}");

                return;
            }

            Console.WriteLine("Enter list id to share");
            string _id = Console.ReadLine();
            int id;
            if (int.TryParse(_id, out id))
            {
            }
            else
            {
                Console.WriteLine("Invalid input");

                return;
            }

            TaskList list = _listService.GetTaskList(id);
            if (list == null)
            {
                Console.WriteLine($"There isn't a list with id: {id}.");
                return;
            }

            _listService.ShareTaskList(receiver, id);
            Console.WriteLine($"You shared list {list.Title}");
        }

        public void PromptShowTaskLists()
        {
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            List<TaskList> lists = _listService.GetAllTaskLists(UserService.CurrentUser);

            foreach (TaskList list in lists)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(list.ToString());
            }

            List<TaskList> sharedLists = _listService.GetSharedLists();

            Console.WriteLine("SharedLists");

            foreach (TaskList list in sharedLists)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(list.ToString());
            }
        }
    }

}

