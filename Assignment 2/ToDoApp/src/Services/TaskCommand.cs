using System;
using System.Collections.Generic;
using System.Text;
using ToDoAppData;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class TaskCommand
    {
        private TaskService taskService;
        private TaskListService listService;
        private UserService userService;

        public TaskCommand(TaskService _taskService, TaskListService _listService, UserService _userService)
        {
            taskService = _taskService;
            listService = _listService;
            userService = _userService;
        }

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

            TaskList list = listService.GetTaskList(id);

            if (list == null)
            {
                Console.WriteLine("The list does not exist");
                return;
            }

            if(list.CreatorId != UserService.CurrentUser.Id)
            {
                Console.WriteLine("You don't have permission to do that");

                return;
            }

            Console.WriteLine("Enter title");
            string title = Console.ReadLine();
            if (String.IsNullOrEmpty(title))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }

            if (taskService.GetTask(title) != null)
            {
                Console.WriteLine($"Task with title {title} already exist");

                return;
            }

            Console.WriteLine("Enter description");
            string description = Console.ReadLine();
            if (String.IsNullOrEmpty(description))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }

            Console.WriteLine("Is complete? yes or no");
            string answer = Console.ReadLine();
            if (String.IsNullOrEmpty(answer))
            {
                Console.WriteLine("You can't enter empty values");

                return;
            }
            bool isComplete = true;
            if (answer.ToLower() == "yes")
            {
                isComplete = true;
            }
            else if (answer.ToLower() == "no")
            {
                isComplete = false;
            }
            else
            {
                Console.WriteLine("Invalid input");

                return;
            }

            taskService.CreateTask(UserService.CurrentUser, list, title, description, isComplete);
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

            TaskList lists = listService.GetTaskList(id);

            if (lists == null)
            {
                Console.WriteLine($"There isn't a list with id: {id}.");
                return;
            }

            List<Task> tasks = taskService.GetTasks(id);

            foreach (Task task in tasks)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine(task.ToString());
            }

            Console.WriteLine("Assigned Tasks");

            List<Task> assignedTasks = taskService.GetTasks(id);

            foreach (Task task in assignedTasks)
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
            TaskList list = listService.GetTaskList(id);

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

            Task currentTask = taskService.GetTask(id);

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

            taskService.EditTask(currentTask.Id, newTitle, newDescription, isComplete);
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

            TaskList list = listService.GetTaskList(id);

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

            TaskList list = listService.GetTaskList(id);

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

            taskService.DeteleTask(id);
        }

        public void PromptAssignTask()
        {
            if (UserService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            Console.WriteLine("Enter list title");
            string listTitle = Console.ReadLine();
            listService.CreateTaskList(UserService.CurrentUser, listTitle);
            TaskList currentList = listService.GetTaskList(listTitle);

            Console.WriteLine("Enter Task Id to assign");
            string _taskId = Console.ReadLine();
            int taskId;
            if (int.TryParse(_taskId, out taskId))
            {
            }
            else
            {
                Console.WriteLine("Invalid input");

                return;
            }

            Task toAssign = taskService.GetTask(taskId);

            if(toAssign == null)
            {
                Console.WriteLine($"The task with id {taskId} doesn't exist");

                return;
            }

            string username;

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

                taskService.AssignTask(receiver, toAssign.Id);

                Console.WriteLine($"You assigned task {toAssign.Title} to user {username}");
            } while (username != "1");
        }
    }
}

