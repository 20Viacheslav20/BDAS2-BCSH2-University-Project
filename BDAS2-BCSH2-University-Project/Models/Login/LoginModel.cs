
using System.ComponentModel.DataAnnotations;

namespace BDAS2_BCSH2_University_Project.Models.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please write login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Please write password")]
        public string Password { get; set; }
    }
}
