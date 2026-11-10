using System.ComponentModel.DataAnnotations;

namespace Task_8.Models
{
    public class LoginDTO
    {
      
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            public string Password { get; set; } = string.Empty;
        }
    }

