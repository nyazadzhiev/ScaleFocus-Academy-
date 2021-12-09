using System.Collections.Generic;

namespace ProjectManagementApp.DAL.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public virtual List<Team> Teams { get; set; }
        public virtual List<ToDoTask> Tasks { get; set; }

        public Project()
        {
            this.Tasks = new List<ToDoTask>();
            this.Teams = new List<Team>();
        }
    }
}