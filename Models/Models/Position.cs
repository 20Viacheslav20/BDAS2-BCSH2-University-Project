using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Position
    {
        public int Id { get; set; }

        [Display(Name = "Position")]
        [Required(ErrorMessage = "Position name is required.")]
        [MinLength(4, ErrorMessage = "Position name must be at least 4 characters.")]
        public string Name { get; set; }

        [Range(1000, int.MaxValue, ErrorMessage = "Salary must be a positive number and more than 1000")]
        public int Salary { get; set; }

        [Display(Name = "Employee count")]
        public int EmployeeCount { get; set; } 
    }
}
