using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoAppEntities
{
    public class SharedList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ListId { get; set; }
        public User User { get; set; }
        public TaskList List { get; set; }
    }
}
