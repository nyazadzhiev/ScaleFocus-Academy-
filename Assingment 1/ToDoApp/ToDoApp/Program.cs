using System;
using System.Collections.Generic;
using ToDoApp.Entities;
using ToDoApp.Services;

namespace ToDoApp
{
    class Program
    {
        private static UserService userService = new UserService();
        private static TaskService taskService = new TaskService();
        private static TaskListService listService = new TaskListService();

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                userService.CreateUser("admin", "adminpassword", "Admin", "Admin", true);
            }
            bool shouldExit = false;
            while (!shouldExit)
            {
                shouldExit = Menu();
            }
        }

        private static bool Menu()
        {
            ShowMenu();

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (userService.CurrentUser == null)
                    {
                        LogIn();
                    }
                    else
                    {
                        LogOut();
                    }
                    break;
                case "2":
                    CreateUser();
                    break;
                case "3":
                    CreateTaskList();
                    break;
                case "4":
                    AddTask();
                    break;
                case "5":
                    ShowTasks();
                    break;
                case "6":
                    EditTask();
                    break;
                case "7":
                    CompleteTask();
                    break;
                case "8":
                    DeleteTask();
                    break;
                case "9":
                    ShareTaskList();
                    break;
                case "10":
                    return true;
            }

            return false;
        }

        private static void ShowMenu()
        {
            Console.WriteLine("---------- Main Menu ----------");
            if (userService.CurrentUser == null)
            {
                Console.WriteLine("1. LogIn ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You are logged in as: {userService.CurrentUser.Username}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1. LogOut");
            }
            Console.WriteLine("2. Create User");
            Console.WriteLine("3. Create TaskList");
            Console.WriteLine("4. Add Task");
            Console.WriteLine("5. Show Tasks");
            Console.WriteLine("6. Edit task");
            Console.WriteLine("7. Complete task");
            Console.WriteLine("8. Delete Task");
            Console.WriteLine("9. Share List");
            Console.WriteLine("10. Exit");
        }

        private static void LogIn()
        {
            Console.WriteLine("Enter your user name:");
            string userName = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            userService.Login(userName, password);
            if (userService.CurrentUser == null)
            {
                Console.WriteLine("Login failed.");
            }
            else
            {
                Console.WriteLine("Login successful.");
            }
        }

        private static void LogOut()
        {
            userService.LogOut();
        }

        private static void CreateUser()
        {
            if (userService.CurrentUser == null || userService.CurrentUser.IsAdmin)
            {
                Console.WriteLine("You don't have permission to this operation");
                return;
            }

            Console.WriteLine("User Name:");
            string name = Console.ReadLine();

            Console.WriteLine("Password:");
            string password = Console.ReadLine();

            Console.WriteLine("First Name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Last Name:");
            string lastName = Console.ReadLine();

            bool isAdmin;
            Console.WriteLine("Enter role (admin or user)");
            string answer = Console.ReadLine();


            if (answer.ToLower() == "admin")
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }
            
            bool isSuccess = userService.CreateUser(name, password, firstName, lastName, isAdmin);
            if (isSuccess)
            {
                Console.WriteLine($"User with name '{name}' added");
            }
            else
            {
                Console.WriteLine($"User with name '{name}' already exists");
            }
        }

        private static bool CreateTaskList()
        {
            if(userService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return false;
            }

            Console.WriteLine("Enter title");
            string title = Console.ReadLine();

            TaskList taskList = listService.CreateTaskList(userService.CurrentUser, title);


            if (taskList != null)
            {
                userService.CurrentUser.ToDoList.Add(taskList);
                Console.WriteLine($"You created a tasklist {title}");

                return true;
            }
            else
            {
                Console.WriteLine("There is no such a user");

                return false;
            }
        }

        private static void AddTask()
        {
            if (userService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");
                return;
            }

            Console.WriteLine("Enter List id:");
            string _id = Console.ReadLine();
            int id = Convert.ToInt32(_id);

            TaskList list = listService.GetTaskList(userService.CurrentUser, id);

            if(list == null)
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
            if(answer.ToLower() == "yes")
            {
                isComplete = true;
            }
            else if(answer.ToLower() == "no")
            {
                isComplete = false;
            }

            taskService.CreateTask(list, userService.CurrentUser, title, description, isComplete);
        }

        private static void ShowTasks()
        {
            if (userService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            Console.WriteLine("Enter id");
             string _id = Console.ReadLine();

             int id = Convert.ToInt32(_id);
            
             TaskList tasks = listService.GetTaskList(userService.CurrentUser, id);

            if(tasks == null)
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

        private static void EditTask()
        {
            if (userService.CurrentUser == null)
            {
                Console.WriteLine("Please Log in");
                return;
            }

            Console.WriteLine("Enter list id");
            string _id = Console.ReadLine();
            int id = Convert.ToInt32(_id);

            TaskList list = listService.GetTaskList(userService.CurrentUser, id);

            if (list == null)
            {
                Console.WriteLine($"There isn't a list with id: {id}.");
                return;
            }

            Console.WriteLine("Enter task id");
            _id = Console.ReadLine();
            id = Convert.ToInt32(_id);

            Task currentTask = taskService.GetTask(list, id);

            if (currentTask == null)
            {
                Console.WriteLine($"There isn't a task with id: {id}.");
                return;
            }

            Console.WriteLine($"You want to edit task {currentTask.Title}");

            Console.WriteLine("Enter new id");
            _id = Console.ReadLine();
            id = Convert.ToInt32(_id);

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

            taskService.EditTask(list, currentTask.Id, id, newTitle, newDescription, isComplete);
        }

        private static void CompleteTask()
        {
            if (userService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            Console.WriteLine("Enter list id");
            string _id = Console.ReadLine();
            int id = Convert.ToInt32(_id);

            TaskList list = listService.GetTaskList(userService.CurrentUser, id);

            if (list == null)
            {
                Console.WriteLine($"There isn't a list with id: {id}.");
                return;
            }

            Console.WriteLine("Enter task id");
            _id = Console.ReadLine();
            id = Convert.ToInt32(_id);

            taskService.CompleteTask(list, id);

            Console.WriteLine("The task was completed");
        }

        private static void DeleteTask()
        {
            if (userService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            Console.WriteLine("Enter list id");
            string _id = Console.ReadLine();
            int id = Convert.ToInt32(_id);

            TaskList list = listService.GetTaskList(userService.CurrentUser, id);

            if (list == null)
            {
                Console.WriteLine($"There isn't a list with id: {id}.");
                return;
            }

            Console.WriteLine("Enter task id");
            _id = Console.ReadLine();
            id = Convert.ToInt32(_id);

            taskService.DeteleTask(list, id);
        }

        private static void ShareTaskList()
        {
            if (userService.CurrentUser == null)
            {
                Console.WriteLine("Please log in");

                return;
            }

            Console.WriteLine("Enter receiver username");
            string username = Console.ReadLine();

            User receiver = userService.getUserByUsername(username);

            Console.WriteLine("Enter list id to share");
            string _id = Console.ReadLine();
            int id = Convert.ToInt32(_id);

            TaskList list = listService.GetTaskList(userService.CurrentUser, id);
            if (list == null)
            {
                Console.WriteLine($"There isn't a list with id: {id}.");
                return;
            }

            receiver.ToDoList.Add(list);
        }
    }
}
