using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class LoginModel
    {
        public Guid Id { get; set; }
        // [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public DateTime LastLoginTime { get; set; }
    }
}
