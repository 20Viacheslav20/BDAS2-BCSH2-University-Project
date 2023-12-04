using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Position
    {
        public int Id { get; set; }

        [Display(Name = "Position")]
        public string Name { get; set; }

        public double Salary { get; set; }

        [Display(Name = "Employee count")]
        public int EmployeeCount { get; set; } 
    }
}
