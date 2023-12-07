using Models.Models.Categor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models.Models.Product
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Actual Price")]
        public double ActualPrice { get; set; }

        [Display(Name = "ClubCard Price")]
        public int? ClubCardPrice { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal Weight { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string Aviability { get; set; }

    }
}

