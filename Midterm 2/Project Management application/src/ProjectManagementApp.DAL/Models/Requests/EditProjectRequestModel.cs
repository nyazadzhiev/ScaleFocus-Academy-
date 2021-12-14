using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagementApp.DAL.Models.Requests
{
    public class EditProjectRequestModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Title { get; set; }
    }
}
