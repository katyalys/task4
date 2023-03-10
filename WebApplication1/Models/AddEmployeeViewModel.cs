using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WebApplication1.Models
{
    public class AddEmployeeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime LastLoginTime { get; set; }
        public DateTime RegisterTime { get; set; }
        public string Status { get; set; }
    }
}
