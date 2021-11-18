using System.Collections.Generic;

namespace ProjectManagementApp.DAL.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public List<Team> Teams { get; set; }
        public List<ToDoTask> Tasks { get; set; }

        public Project()
        {
            this.Tasks = new List<ToDoTask>();
            this.Teams = new List<Team>();
        }
    }
}