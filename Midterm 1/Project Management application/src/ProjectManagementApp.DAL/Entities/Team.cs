using System.Collections.Generic;

namespace ProjectManagementApp.DAL.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public List<Project> Projects { get; set; }

        public Team()
        {
            this.Users = new List<User>();
            this.Projects = new List<Project>();
        }
    }
}