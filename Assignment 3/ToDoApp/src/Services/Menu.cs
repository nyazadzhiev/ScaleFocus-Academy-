using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoAppData;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class Menu
    {
        private TaskCommand taskCommand;
        private TaskListCommand listCommand;
        private UserService userService;
        private TaskListService listService;
        private TaskService taskService;
        private UserCommand userCommand;
        private Validations validations;

        public Menu(DatabaseContext database)
        {
            userService = new UserService(database);
            listService = new TaskListService(database);
            taskService = new TaskService(database);
            userCommand = new UserCommand(userService);
            listCommand = new TaskListCommand(userService, listService);
            taskCommand = new TaskCommand(taskService, listService, userService);
            validations = new Validations();
        }

        public async Task<bool> MainMenu(User currentUser)
        {
            ShowMenu(currentUser);

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (currentUser == null)
                    {
                        await userCommand.PromptLogIn();
                    }
                    else
                    {
                        userCommand.PromptLogOut();
                    }
                    break;
                case "2":
                    bool isPrintSuccesfull = PrintUserManagementMenu(currentUser);

                    if (isPrintSuccesfull)
                    {
                        string innerChoice = Console.ReadLine();
                        switch (innerChoice)
                        {
                            case "1":
                                await userCommand.PromptCreateUser();
                                break;
                            case "2":
                                userCommand.PromptGetAllUsers();
                                break;
                            case "3":
                                await userCommand.PromptEditUser();
                                break;
                            case "4":
                                await userCommand.PromptDeleteUser();
                                break;
                        }
                    }
                    return false;

                case "3":
                    isPrintSuccesfull = PrintTaskListManagementMenu(currentUser);

                    if (isPrintSuccesfull)
                    {
                        string innerChoice = Console.ReadLine();

                        switch (innerChoice)
                        {
                            case "1":
                                await listCommand.PromptCreateTaskList();
                                break;
                            case "2":
                                listCommand.PromptShowTaskLists();
                                break;
                            case "3":
                                await listCommand.PromptEditTaskList();
                                break;
                            case "4":
                                await listCommand.PromptDeleteTaskList();
                                break;
                            case "5":
                                await listCommand .PromptShareTaskList();
                                break;
                        }
                    }
                    return false;

                case "4":
                    isPrintSuccesfull = PrintTaskManagementMenu(currentUser);

                    if (isPrintSuccesfull)
                    {
                        string innerChoice = Console.ReadLine();
                        switch (innerChoice)
                        {
                            case "1":
                                await taskCommand.PromptAddTask();
                                break;
                            case "2":
                                await taskCommand.PromptAssignTask();
                                break;
                            case "3":
                                taskCommand.PromptShowTasks();
                                break;
                            case "4":
                                await taskCommand.PromptEditTask();
                                break;
                            case "5":
                                await taskCommand.PromptCompleteTask();
                                break;
                            case "6":
                                await taskCommand .PromptDeleteTask();
                                break;
                        }
                    }
                    return false;

                case "5":
                    return true;
            }

            return false;
        }

        private static void ShowMenu(User currentUser)
        {
            LogMenu(currentUser);
            if (currentUser != null)
            {
                if (currentUser.IsAdmin)
                {
                    Console.WriteLine("2. User Management Menu");
                    Console.WriteLine("3. Tasklist Management Menu");
                    Console.WriteLine("4. Task Management Menu");
                }
                else
                {
                    Console.WriteLine("3. Tasklist Management Menu");
                    Console.WriteLine("4. Task Management Menu");
                }
            }
            Console.WriteLine("5. Exit");
        }

        private static void LogMenu(User currentUser)
        {
            Console.WriteLine("---------- Main Menu ----------");
            if (currentUser == null)
            {
                Console.WriteLine("1. LogIn ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You are logged in as: {currentUser.Username}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1. LogOut");
            }
        }

        private bool PrintUserManagementMenu(User user)
        {
            bool isLoggedIn = validations.CheckLog(user);
            if (!isLoggedIn)
            {
                return false;
            }

            bool isAdmin = validations.IsAdmin(user);
            if (!isAdmin)
            {
                return false;
            }
            else
            {
                Console.WriteLine("---------- User Management Menu ----------");
                Console.WriteLine("1. Create User");
                Console.WriteLine("2. Show Users List");
                Console.WriteLine("3. Edit User");
                Console.WriteLine("4. Delete User");
                Console.WriteLine("5. Go back");

                return true;
            }
        }

        private bool PrintTaskListManagementMenu(User user)
        {
            bool isLoggedIn = validations.CheckLog(user);
            if (!isLoggedIn)
            {
                return false;
            }
            else
            {
                Console.WriteLine("---------- Tasklist Management Menu ----------");
                Console.WriteLine("1. Create TaskList");
                Console.WriteLine("2. Show TaskLists");
                Console.WriteLine("3. Edit TaskList");
                Console.WriteLine("4. Delete TaskLists");
                Console.WriteLine("5. Share List");
                Console.WriteLine("6. Go Back");

                return true;
            }
        }

        private bool PrintTaskManagementMenu(User user)
        {
            bool isLoggedIn = validations.CheckLog(user);
            if (!isLoggedIn)
            {
                return false;
            }
            else
            {
                Console.WriteLine("---------- Task Management Menu ----------");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. Assign Task");
                Console.WriteLine("3. Show Tasks");
                Console.WriteLine("4. Edit task");
                Console.WriteLine("5. Complete task");
                Console.WriteLine("6. Delete Task");
                Console.WriteLine("7. Go back");

                return true;
            }
        }
    }
}

