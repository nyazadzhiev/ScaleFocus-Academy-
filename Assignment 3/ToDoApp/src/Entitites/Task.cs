using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAppEntities
{
    public class Task : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
        public int ListId { get; set; }
        public TaskList ToDoList { get; set; }

        public override string ToString()
        {
            return $"Id {this.Id} \n" +
                $"Title: {this.Title} \n" +
                $"Description: {this.Description} \n" +
                $"Is completed: {this.IsComplete} \n" +
                $"Created at {this.CreatedAt}\n" +
                $"Last Edited: {this.LastEdited}\n";
        }
    }
}
