using Models.Models.Categor;
using Models.Validators;
using System.ComponentModel.DataAnnotations;


namespace Models.Models.Product
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Name must be at least 4 characters.")]
        public string Name { get; set; }


        [Display(Name = "Actual Price")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Actual Price must be a non-negative number.")]
        public double ActualPrice { get; set; }

        [Display(Name = "ClubCard Price")]
        [Range(0.1, double.MaxValue, ErrorMessage = "ClubCard Price must be a non-negative number.")]
        [ClubCardPriceLessThanActualPrice(nameof(ActualPrice))]
        public double? ClubCardPrice { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Weight must be a non-negative number.")]
        public double Weight { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category.")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string Aviability { get; set; }

    }
}

