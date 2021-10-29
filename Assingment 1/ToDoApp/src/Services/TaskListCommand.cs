using System;
using System.Collections.Generic;
using System.Text;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class TaskListCommand
    {
        UserService userService = new UserService();
        TaskListService listService = new TaskListService();

        public void PromptCreateTaskList()
        {
            Console.WriteLine("Enter title");
            string title = Console.ReadLine();

            TaskList taskList = listService.CreateTaskList(UserService.CurrentUser, title);

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

            TaskList list = listService.GetTaskList(UserService.CurrentUser, id);

            if (list == null)
            {
                Console.WriteLine($"There isn't a list with id: {id}.");
                return;
            }

            Console.WriteLine($"You want to edit list {list.Title}");

            Console.WriteLine("Enter new title");
            string newTitle = Console.ReadLine();

            listService.EditTaskList(id, newTitle);
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

            listService.DeleteTaskList(id);
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

            User receiver = userService.GetUser(username);

            if (receiver == null)
            {
                Console.WriteLine($"There isn't user with username {username}");

                return;
            }

            Console.WriteLine("Enter list id to share");
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

            TaskList list = listService.GetTaskList(UserService.CurrentUser, id);
            if (list == null)
            {
                Console.WriteLine($"There isn't a list with id: {id}.");
                return;
            }

            receiver.ToDoList.Add(list);
            Console.WriteLine($"You shared list {list.Title}");
        }

        public void PromptShowTaskLists()
        {
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            foreach (TaskList list in UserService.CurrentUser.ToDoList)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(list.ToString());
            }
        }
    }

}

