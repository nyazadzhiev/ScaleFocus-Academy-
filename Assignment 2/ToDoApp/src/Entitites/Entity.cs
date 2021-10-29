using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAppEntities
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastEdited { get; set; }
    }
}
