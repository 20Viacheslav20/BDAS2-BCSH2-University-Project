
using System.ComponentModel.DataAnnotations;

namespace Models.Models.Categor
{
    public class IncreasePrice
    {
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid value.")]
        public int Percent { get; set; }

        public Category Category { get; set; }
    }
}
