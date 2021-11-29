using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagementApp.DAL.Models.Requests
{
    public class UserRequestModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        public bool IsAdmin { get; set; }
    }
}
