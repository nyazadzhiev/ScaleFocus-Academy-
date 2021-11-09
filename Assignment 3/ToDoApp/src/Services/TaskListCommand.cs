using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class TaskListCommand
    {
        UserService _userService;
        TaskListService _listService;
        UserInput userInput;
        Validations validations;

        public TaskListCommand(UserService userService, TaskListService listService)
        {
            _userService = userService;
            _listService = listService;
            userInput = new UserInput();
            validations = new Validations();
        }

        public async Task PromptCreateTaskList()
        {
            try
            {
                string title = userInput.EnterValue("title");

                if (_listService.GetTaskList(title) != null)
                {
                    Console.WriteLine($"List with title {title} already exist");

                    return;
                }

                await _listService.CreateTaskList(UserService.CurrentUser, title);

                Console.WriteLine($"You created a tasklist {title}");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
        }

        public async Task PromptEditTaskList()
        {
            try
            {
                int id = userInput.EnterId("List Id");

                TaskList list = _listService.GetTaskList(id);

                bool isValidList = validations.EnsureListExist(list);
                if (!isValidList)
                {
                    return;
                }

                Console.WriteLine($"You want to edit list {list.Title}");


                string newTitle = userInput.EnterValue("new title");

                await _listService.EditTaskList(id, newTitle);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
        }

        public async Task PromptDeleteTaskList()
        {
            try
            {
                int id = userInput.EnterId("List Id");

                await _listService.DeleteTaskList(id);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Invalid input");
                Console.WriteLine(e.Message);
            }
        }

        public async Task PromptShareTaskList()
        {
            try
            {
                string username = userInput.EnterValue("receiver username");

                if (username == UserService.CurrentUser.Username)
                {
                    Console.WriteLine("You can't share with yourself");

                    return;
                }

                User receiver = _userService.GetUser(username);

                bool isValidUser = validations.EnsureUserExist(receiver);

                if (!isValidUser)
                {
                    return;
                }

                int id = userInput.EnterId("List Id to share");

                TaskList list = _listService.GetTaskList(id);
                bool isValidList = validations.EnsureListExist(list);
                if (!isValidList)
                {
                    return;
                }

                await _listService.ShareTaskList(receiver, id);
                Console.WriteLine($"You shared list {list.Title}");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid input");
            }
        }

        public void PromptShowTaskLists()
        {
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

