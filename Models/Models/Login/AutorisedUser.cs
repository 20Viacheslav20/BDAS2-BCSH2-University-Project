using Models.Validators;
using System.ComponentModel.DataAnnotations;

namespace Models.Models.Login
{
    public class AutorisedUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please write login")]
        public string Login { get; set; }

        [RequiredIf(nameof(Id), 0, ErrorMessage = "Please write password")]
        public string Password { get; set; }

        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public List<UserRole> Roles { get; set; }
        public Image Image { get; set; }
    }
}
