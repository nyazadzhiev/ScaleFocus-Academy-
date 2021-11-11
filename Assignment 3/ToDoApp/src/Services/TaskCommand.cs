using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public async Task PromptAddTask()
        {
            try
            {
                int id = userInput.EnterId("List Id");

                TaskList list = listService .GetTaskList(id);

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

                bool isComplete = userInput.EnterTaskCompleted();

                await taskService.CreateTask(UserService.CurrentUser, list, title, description, isComplete);
                Console.WriteLine($"You created task {title}");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
        }

        public async Task PromptShowTasks()
        {
            try
            {
                int id = userInput.EnterId("List Id");

                TaskList list = listService .GetTaskList(id);

                bool isValidList = validations.EnsureListExist(list);
                if (!isValidList)
                {
                    return;
                }

                List<ToDoTask> tasks = taskService.GetTasks(id);

                foreach (ToDoTask task in tasks)
                {
                    Console.WriteLine("--------------------------");
                    Console.WriteLine(task.ToString());
                }

                Console.WriteLine("Assigned Tasks");

                List<ToDoTask> assignedTasks = taskService.GetAssignedTasks();

                foreach (ToDoTask task in assignedTasks)
                {
                    Console.WriteLine("--------------------------");
                    Console.WriteLine(task.ToString());
                }
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
        }

        public async Task PromptEditTask()
        {
            try
            {
                int id = userInput.EnterId("List Id");
                TaskList list = listService .GetTaskList(id);

                bool isValidList = validations.EnsureListExist(list);
                if (!isValidList)
                {
                    return;
                }

                id = userInput.EnterId("Task Id");

                ToDoTask currentTask = taskService.GetTask(id);

                bool isValidTask = validations.EnsureTaskExist(currentTask);
                if (!isValidTask)
                {
                    return;
                }

                Console.WriteLine($"You want to edit task {currentTask.Title}");


                string newTitle = userInput.EnterValue("new title");

                string newDescription = userInput.EnterValue("new description");

                bool isComplete = userInput.EnterTaskCompleted();

                await taskService.EditTask(currentTask.Id, newTitle, newDescription, isComplete);
                Console.WriteLine("You successfully edited task");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("You don't have permission to do this");
            }
        }

        public async Task PromptCompleteTask()
        {
            try
            {
                int id = userInput.EnterId("List Id");

                TaskList list = listService .GetTaskList(id);

                bool isValidList = validations.EnsureListExist(list);
                if (!isValidList)
                {
                    return;
                }

                id = userInput.EnterId("Task Id");

                await taskService .CompleteTask(list, id);

                Console.WriteLine("The task was completed");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("You don't have permission to do this");
            }
        }

        public async Task PromptDeleteTask()
        {
            try
            {
                int id = userInput.EnterId("List Id");

                TaskList list = listService .GetTaskList(id);

                bool isValidList = validations.EnsureListExist(list);
                if (!isValidList)
                {
                    return;
                }

                id = userInput.EnterId("Task Id");

                await taskService.DeteleTask(id);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("You don't have permission to do this");
            }
        }

        public async Task PromptAssignTask()
        {
            try
            {
                string listTitle = userInput.EnterValue("new title");

                await listService .CreateTaskList(UserService.CurrentUser, listTitle);
                TaskList currentList = listService.GetTaskList(listTitle);

                int taskId = userInput.EnterId("Task Id");

                ToDoTask toAssign = taskService.GetTask(taskId);

                bool isValidTask = validations.EnsureTaskExist(toAssign);
                if (!isValidTask)
                {
                    return;
                }

                string username;

                do
                {
                    username = userInput.EnterValue("receiver username or 1 to exit");

                    User receiver = userService .GetUser(username);

                    bool isValidUser = validations.EnsureUserExist(receiver);
                    if (!isValidUser)
                    {
                        return;
                    }

                    await taskService .AssignTask(receiver, toAssign.Id);
                    Console.WriteLine($"You assigned task {toAssign.Title} to user {username}");

                } while (username != "1");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("You don't have permission to do this");
            }
        }
    }
}

