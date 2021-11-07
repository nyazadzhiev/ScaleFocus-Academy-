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
        private UserInput userInput;
        private Validations validations;

        public TaskCommand(TaskService _taskService, TaskListService _listService, UserService _userService)
        {
            taskService = _taskService;
            listService = _listService;
            userService = _userService;
            userInput = new UserInput();
            validations = new Validations();
        }

        public void PromptAddTask()
        {
            try
            {
                int id = userInput.EnterId("List Id");

                TaskList list = listService.GetTaskList(id);

                bool isValidList = validations.EnsureListExist(list);
                if (!isValidList)
                {
                    return;
                }

                if (list.CreatorId != UserService.CurrentUser.Id)
                {
                    Console.WriteLine("You don't have permission to do that");

                    return;
                }


                string title = userInput.EnterValue("title");

                if (taskService.GetTask(title) != null)
                {
                    Console.WriteLine($"Task with title {title} already exist");

                    return;
                }

                string description = userInput.EnterValue("description");

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
            catch
            {
                Console.WriteLine("Invalid input");
            }
        }

        public void PromptShowTasks()
        {
            try
            {
                int id = userInput.EnterId("List Id");

                TaskList list = listService.GetTaskList(id);

                bool isValidList = validations.EnsureListExist(list);
                if (!isValidList)
                {
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
            catch
            {
                Console.WriteLine("Invalid input");
            }
        }

        public void PromptEditTask()
        {
            try
            {
                int id = userInput.EnterId("List Id");
                TaskList list = listService.GetTaskList(id);

                bool isValidList = validations.EnsureListExist(list);
                if (!isValidList)
                {
                    return;
                }

                id = userInput.EnterId("Task Id");

                Task currentTask = taskService.GetTask(id);

                bool isValidTask = validations.EnsureTaskExist(currentTask);
                if (!isValidTask)
                {
                    return;
                }

                Console.WriteLine($"You want to edit task {currentTask.Title}");


                string newTitle = userInput.EnterValue("new title");

                string newDescription = userInput.EnterValue("new description");

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
            catch
            {
                Console.WriteLine("Invalid input");
            }
        }

        public void PromptCompleteTask()
        {
            try
            {
                int id = userInput.EnterId("List Id");

                TaskList list = listService.GetTaskList(id);

                bool isValidList = validations.EnsureListExist(list);
                if (!isValidList)
                {
                    return;
                }

                id = userInput.EnterId("Task Id");

                taskService.CompleteTask(list, id);

                Console.WriteLine("The task was completed");
            }
            catch
            {
                Console.WriteLine("Invalid input");
            }
        }

        public void PromptDeleteTask()
        {
            try
            {
                int id = userInput.EnterId("List Id");

                TaskList list = listService.GetTaskList(id);

                bool isValidList = validations.EnsureListExist(list);
                if (!isValidList)
                {
                    return;
                }

                id = userInput.EnterId("Task Id");

                taskService.DeteleTask(id);
            }
            catch
            {
                Console.WriteLine("Invalid input");
            }
        }

        public void PromptAssignTask()
        {
            try
            {
                string listTitle = userInput.EnterValue("new title");

                listService.CreateTaskList(UserService.CurrentUser, listTitle);
                TaskList currentList = listService.GetTaskList(listTitle);

                int taskId = userInput.EnterId("Task Id");

                Task toAssign = taskService.GetTask(taskId);

                bool isValidTask = validations.EnsureTaskExist(toAssign);
                if (!isValidTask)
                {
                    return;
                }

                string username;

                do
                {
                    username = userInput.EnterValue("receiver username or 1 to exit");

                    User receiver = userService.GetUser(username);

                    bool isValidUser = validations.EnsureUserExist(receiver);
                    if (!isValidUser)
                    {
                        return;
                    }

                    taskService.AssignTask(receiver, toAssign.Id);
                    Console.WriteLine($"You assigned task {toAssign.Title} to user {username}");

                } while (username != "1");
            }
            catch
            {
                Console.WriteLine("Invalid input");
            }
        }
    }
}

