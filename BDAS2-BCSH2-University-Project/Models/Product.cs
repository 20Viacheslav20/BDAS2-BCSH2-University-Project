using System.ComponentModel.DataAnnotations;

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
        public int CaregoryId { get; set; }

        [Range(0.1, 100)]
        public decimal Weight { get; set; }

    }
}

