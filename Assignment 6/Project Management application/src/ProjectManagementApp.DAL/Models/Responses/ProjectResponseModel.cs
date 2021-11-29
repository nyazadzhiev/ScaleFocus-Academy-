using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.DAL.Models.Responses
{
    public class ProjectResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int OwnerId { get; set; }
    }
}
