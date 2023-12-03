using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        public string StringAddress => $"{City} - {Street}";
    }
}
