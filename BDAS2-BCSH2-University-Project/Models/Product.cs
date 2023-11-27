using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDAS2_BCSH2_University_Project.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Actual Price")]
        public int ActualPrice { get; set; }

        [Display(Name = "ClubCard Price")]
        public int? ClubCardPrice { get; set; }  
        public Category Category { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal Weight { get; set; }

    }
}

