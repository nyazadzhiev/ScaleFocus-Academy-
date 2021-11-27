using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagementApp.DAL.Models.Requests
{
    public class WorkLogRequestModel
    {
        [Required]
        public int WorkedHours { get; set; }
    }
}
