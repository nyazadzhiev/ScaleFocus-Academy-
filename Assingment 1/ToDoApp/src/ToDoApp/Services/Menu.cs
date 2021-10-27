using System;
using System.Collections.Generic;
using System.Text;
using ToDoApp.Entities;

namespace ToDoApp.Services
{
    public class Menu
    {
        private static TaskCommand taskCommand = new TaskCommand();
        private static TaskListCommand listCommand = new TaskListCommand();
        private static UserCommand userCommand = new UserCommand();

        public bool MainMenu(User currentUser)
        {
            ShowMenu(currentUser);

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (currentUser == null)
                    {
                        userCommand.PromptLogIn();
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
                                userCommand.PromptCreateUser();
                                break;
                            case "2":
                                userCommand.PromptGetAllUsers();
                                break;
                            case "3":
                                userCommand.PromptEditUser();
                                break;
                            case "4":
                                userCommand.PromptDeleteUser();
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
                                listCommand.PromptCreateTaskList();
                                break;
                            case "2":
                                listCommand.PromptShowTaskLists();
                                break;
                            case "3":
                                listCommand.PromptDeleteTaskList();
                                break;
                            case "4":
                                listCommand.PromptShareTaskList();
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
                                taskCommand.PromptAddTask();
                                break;
                            case "2":
                                taskCommand.PromptShowTasks();
                                break;
                            case "3":
                                taskCommand.PromptEditTask();
                                break;
                            case "4":
                                taskCommand.PromptCompleteTask();
                                break;
                            case "5":
                                taskCommand.PromptDeleteTask();
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
            if (user == null)
            {
                Console.WriteLine("Please Log In");

                return false;
            }

            if (!user.IsAdmin)
            {
                Console.WriteLine("You don't have permission to enter this menu");

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
            if (user == null)
            {
                Console.WriteLine("Please Log In");

                return false;
            }
            else
            {
                Console.WriteLine("---------- Tasklist Management Menu ----------");
                Console.WriteLine("1. Create TaskList");
                Console.WriteLine("2. Show TaskLists");
                Console.WriteLine("3. Delete TaskLists");
                Console.WriteLine("4. Share List");
                Console.WriteLine("5. Go Back");

                return true;
            }
        }

        private bool PrintTaskManagementMenu(User user)
        {
            if (user == null)
            {
                Console.WriteLine("Please Log In");

                return false;
            }
            else
            {
                Console.WriteLine("---------- Task Management Menu ----------");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. Show Tasks");
                Console.WriteLine("3. Edit task");
                Console.WriteLine("4. Complete task");
                Console.WriteLine("5. Delete Task");
                Console.WriteLine("6. Go back");

                return true;
            }
        }
    }
}

