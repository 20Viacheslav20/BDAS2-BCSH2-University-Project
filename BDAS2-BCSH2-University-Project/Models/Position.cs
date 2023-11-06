using System.ComponentModel.DataAnnotations;

namespace BDAS2_BCSH2_University_Project.Models
{
    public class Position
    {
        public int Id { get; set; }

        [Display(Name = "Position name: ")]
        public string Name { get; set; }

        public double Salary { get; set; }
    }
}
