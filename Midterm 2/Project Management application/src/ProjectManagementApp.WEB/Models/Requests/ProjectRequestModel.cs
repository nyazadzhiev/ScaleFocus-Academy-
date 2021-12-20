using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagementApp.WEB.Models.Requests
{
    public class ProjectRequestModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Title { get; set; }
    }
}
