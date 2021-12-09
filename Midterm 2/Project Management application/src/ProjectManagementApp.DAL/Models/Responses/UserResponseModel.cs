using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagementApp.DAL.Models.Responses
{
    public class UserResponseModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
