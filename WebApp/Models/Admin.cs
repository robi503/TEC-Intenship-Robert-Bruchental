using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Admin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
