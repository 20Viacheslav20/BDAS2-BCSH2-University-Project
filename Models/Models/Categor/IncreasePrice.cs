
namespace Models.Models.Categor
{
    public class IncreasePrice
    {
        public int CategoryId { get; set; }

        public int Percent { get; set; }

        public Category Category { get; set; }
    }
}
