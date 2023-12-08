using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [MinLength(4, ErrorMessage = "City must be at least 4 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        [MinLength(4, ErrorMessage = "Street must be at least 4 characters.")]
        public string Street { get; set; }

        public string StringAddress => $"{City} - {Street}";
    }
}
