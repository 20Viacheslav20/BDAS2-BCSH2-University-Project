using System.ComponentModel.DataAnnotations;

namespace BDAS2_BCSH2_University_Project.Models.Login
{
    public class AutorisedUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please write login")]
        public string Login { get; set; }

        [Required]
        // TODO add validation 
        public string Password { get; set; }

        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public List<UserRole> Roles { get; set; }

    }
}
