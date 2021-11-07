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
            Console.WriteLine("Is complete? yes or no");
            string answer = Console.ReadLine();

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

                return false;
            }

            return isComplete;
        }

        public bool EnterRole()
        {
            Console.WriteLine("Enter role (admin or user)");
            string answer = Console.ReadLine();

            bool isAdmin;

            if (answer.ToLower() == "admin")
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }

            return isAdmin;
        }
    }
}
