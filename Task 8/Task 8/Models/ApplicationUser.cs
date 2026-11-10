using Microsoft.AspNetCore.Identity;

namespace Task_8.Models
{
   
        public class ApplicationUser : IdentityUser
        {
            public string FullName { get; set; } = string.Empty;
        }
    }

