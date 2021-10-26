﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoApp.Entities;

namespace ToDoApp.Services
{
    class TaskListService
    {
        List<TaskList> tasks = new List<TaskList>();
        private int listIDGenerator = 0;

        public List<TaskList> GetAllTaskLists()
        {
            return tasks;
        }

        public TaskList CreateTaskList(User user, string title)
        {
            listIDGenerator++;

            TaskList newTaskList = new TaskList()
            {
                Id = listIDGenerator,
                Title = title,
                Owner = user
            };

            user.ToDoList.Add(newTaskList);
            tasks.Add(newTaskList);

            return newTaskList;

        }

        public TaskList GetTaskList(User user, int id)
        {
            return user.ToDoList.FirstOrDefault(t => t.Id == id);
        }

        public bool DeleteTaskList(int id)
        {
            TaskList currentList = GetTaskList(UserService.CurrentUser, id);
            if(currentList == null)
            {
                Console.WriteLine($"There isn't list with id {id}");

                return false;
            }
            else
            {
                UserService.CurrentUser.ToDoList.Remove(currentList);
                Console.WriteLine($"You deleted list {currentList.Title}");

                return true;
            }
        }
    }
}

