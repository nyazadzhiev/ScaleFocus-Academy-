using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class UserInput
    {
        public string EnterValue(string parameter)
        {
            Console.WriteLine($"Enter {parameter}");
            string value = Console.ReadLine();

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
                Console.WriteLine("Invalid input");

                return -1;
            }
        }
    }
}
