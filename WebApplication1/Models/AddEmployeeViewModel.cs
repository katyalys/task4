using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AddEmployeeViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime LastLoginTime { get; set; }
        public DateTime RegisterTime { get; set; }
        public string Status { get; set; }

        //[Compare(nameof(Password), ErrorMessage = "Passwords mismatch")]
        //[Compare("Password", ErrorMessage = "Passwords mismatch")]
        //public string ConfirmPassword { get; set; }
    }
}
