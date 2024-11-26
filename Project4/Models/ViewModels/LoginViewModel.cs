using System.ComponentModel.DataAnnotations;

namespace Project4.Models.ViewModels
{
    public class LoginViewModel
    {
        public PasswordHasher hasher { get; set; }

        public Agent agent { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string SaveCookie { get; set; }
    }
}
