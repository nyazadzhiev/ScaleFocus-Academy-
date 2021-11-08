using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAppServices
{
    public class UserInput
    {
        private Validations validations = new Validations();

        public string EnterValue(string parameter)
        {
            Console.WriteLine($"Enter {parameter}");
            string value = Console.ReadLine();

            bool isEmpty = validations.CheckForEmptyInput(value);

            if (isEmpty)
            {
                throw new ArgumentNullException("Invalid input");
            }

            return value;
        }

        public int EnterId(string parameter)
        {
            Console.WriteLine($"Enter {parameter}:");
            string _id = Console.ReadLine();
            int id;
            if (int.TryParse(_id, out id))
            {
                return id;
            }
            else
            {
                throw new ArgumentNullException("Invalid input");
            }
        }

        public bool EnterTaskCompleted()
        {
            Console.WriteLine("Is complete? 1. yes or 2. no");
            string answer = Console.ReadLine();

            bool isComplete = true;
            if (answer.ToLower() == "1")
            {
                isComplete = true;
            }
            else if (answer.ToLower() == "2")
            {
                isComplete = false;
            }
            else
            {
                Console.WriteLine("Invalid input");

                return false;
            }

            return isComplete;
        }

        public bool EnterRole()
        {
            Console.WriteLine("Enter role: 1. admin or 2. user)");
            string answer = Console.ReadLine();

            bool isAdmin;

            if (answer.ToLower() == "1")
            {
                isAdmin = true;
            }
            else if(answer.ToLower() == "2")
            {
                isAdmin = false;
            }
            else
            {
                Console.WriteLine("Invalid input");

                return false;
            }

            return isAdmin;
        }
    }
}
