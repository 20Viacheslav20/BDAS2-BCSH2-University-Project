using System.ComponentModel.DataAnnotations;

namespace BDAS2_BCSH2_University_Project.Models.Login
{
    public class RegistrateModel
    {
        [Required(ErrorMessage = "Please write login")]
        public string Login { get; set; }

        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required]
        // TODO add validation 
        public string Password { get; set; }
        public Employee Employee { get; set; } 
    }
}
