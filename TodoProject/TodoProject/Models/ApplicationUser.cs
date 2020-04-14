using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TodoProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }

        public virtual ICollection<TodoItem> TodoItems { get; set; }
    }
}
