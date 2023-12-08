using System.ComponentModel.DataAnnotations;

namespace Models.Models.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please write login")]
        [MinLength(4, ErrorMessage = "Login must be at least 4 characters.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Please write password")]
        public string Password { get; set; }
    }
}
