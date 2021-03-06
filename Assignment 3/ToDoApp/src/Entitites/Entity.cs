using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoAppEntities
{
    public abstract class Entity
    {
        public int? CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public int? ModifierId { get; set; }
        public virtual User Modifier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastEdited { get; set; }
    }
}
