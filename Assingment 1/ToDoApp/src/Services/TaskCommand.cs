using System;
using System.Collections.Generic;
using System.Text;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class TaskCommand
    {
        private static TaskService taskService = new TaskService();
        private static TaskListService listService = new TaskListService();
        private static UserService userService = new UserService();

        public void PromptAddTask()
        {
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");
                return;
            }

            Console.WriteLine("Enter List id:");
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
                Console.WriteLine("The list does not exist");
                return;
            }

            Console.WriteLine("Enter title");
            string title = Console.ReadLine();

            Console.WriteLine("Enter description");
            string description = Console.ReadLine();

            Console.WriteLine("Is complete? yes or no");
            string answer = Console.ReadLine();
            bool isComplete = true;
            if (answer.ToLower() == "yes")
            {
                isComplete = true;
            }
            else if (answer.ToLower() == "no")
            {
                isComplete = false;
            }

            taskService.CreateTask(list, UserService.CurrentUser, title, description, isComplete);
            Console.WriteLine($"You created task {title}");
        }

        public void PromptShowTasks()
        {
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            Console.WriteLine("Enter list id");
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

            TaskList tasks = listService.GetTaskList(UserService.CurrentUser, id);

            if (tasks == null)
            {
                Console.WriteLine($"There isn't a list with id: {id}.");
                return;
            }

            foreach (Task task in tasks.Tasks)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(task.ToString());
            }
        }

        public void PromptEditTask()
        {
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Please Log in");
                return;
            }

            Console.WriteLine("Enter list id");
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

            Console.WriteLine("Enter task id");
           _id = Console.ReadLine();
            if (int.TryParse(_id, out id))
            {
            }
            else
            {
                Console.WriteLine("Invalid input");

                return;
            }

            Task currentTask = taskService.GetTask(list, id);

            if (currentTask == null)
            {
                Console.WriteLine($"There isn't a task with id: {id}.");
                return;
            }

            Console.WriteLine($"You want to edit task {currentTask.Title}");

            Console.WriteLine("Enter new title");
            string newTitle = Console.ReadLine();

            Console.WriteLine("Enter new description");
            string newDescription = Console.ReadLine();

            Console.WriteLine("Is completed? yes or no");
            string answer = Console.ReadLine();
            bool isComplete = false;
            if (answer.ToLower() == "yes")
            {
                isComplete = true;
            }
            else if (answer.ToLower() == "no")
            {
                isComplete = false;
            }

            taskService.EditTask(list, currentTask.Id, newTitle, newDescription, isComplete);
            Console.WriteLine("You successfully edited task");
        }

        public void PromptCompleteTask()
        {
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            Console.WriteLine("Enter list id");
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

            Console.WriteLine("Enter task id");
            _id = Console.ReadLine();
            if (int.TryParse(_id, out id))
            {
            }
            else
            {
                Console.WriteLine("Invalid input");

                return;
            }

            taskService.CompleteTask(list, id);

            Console.WriteLine("The task was completed");
        }

        public void PromptDeleteTask()
        {
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            Console.WriteLine("Enter list id");
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

            Console.WriteLine("Enter task id");
            _id = Console.ReadLine();
            if (int.TryParse(_id, out id))
            {
            }
            else
            {
                Console.WriteLine("Invalid input");

                return;
            }

            taskService.DeteleTask(list, id);
        }

        public void PromptAssignTask()
        {
            if(UserService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            string username;

            Console.WriteLine("Enter list title");
            string listTitle = Console.ReadLine();

            Console.WriteLine("Enter task title");
            string title = Console.ReadLine();

            Console.WriteLine("Enter description");
            string description = Console.ReadLine();

            Console.WriteLine("Is complete? yes or no");
            string answer = Console.ReadLine();
            bool isComplete = true;
            if (answer.ToLower() == "yes")
            {
                isComplete = true;
            }
            else if (answer.ToLower() == "no")
            {
                isComplete = false;
            }

            do
            {
                Console.WriteLine("Enter receiver's username or 1 to end");
                username = Console.ReadLine();

                User receiver = userService.GetUser(username);

                if (receiver == null)
                {
                    Console.WriteLine($"User with username {username} doesn't exist");

                    return;
                }

                TaskList list = listService.CreateTaskList(receiver, listTitle);
                receiver.ToDoList.Add(list);
                taskService.CreateTask(list, UserService.CurrentUser, title, description, isComplete);

                Console.WriteLine($"You assigned task {title} to user {username}");
            } while (username != "1");
        }
    }
}
