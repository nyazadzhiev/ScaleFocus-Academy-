using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAppEntities
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        public int? ModifierId { get; set; }
        public User Modifier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastEdited { get; set; }
    }
}
