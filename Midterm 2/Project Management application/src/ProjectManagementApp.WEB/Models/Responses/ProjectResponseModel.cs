using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.WEB.Models.Responses
{
    public class ProjectResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }
    }
}
