using System;
using System.Collections.Generic;
using System.Text;
using ToDoAppEntities;

namespace ToDoAppServices
{
    public class Validations
    {
        public bool CheckLog(User user)
        {
            if (user == null)
            {
                Console.WriteLine("Please Log In");

                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckForEmptyInput(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAdmin(User user)
        {
            if (!user.IsAdmin)
            {
                Console.WriteLine("You don't have permission to enter this menu");

                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EnsureUserExist(User user)
        {
            if (user == null)
            {
                Console.WriteLine("The user doesn't exist");

                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EnsureListExist(TaskList list)
        {
            if (list == null)
            {
                Console.WriteLine($"The list doesn't exist");

                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EnsureTaskExist(Task task)
        {
            if (task == null)
            {
                Console.WriteLine($"The task doesn't exist");

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
