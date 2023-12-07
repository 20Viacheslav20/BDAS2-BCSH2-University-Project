using System.ComponentModel.DataAnnotations;

namespace Models.Models.Stands
{
    public class ShopStand
    {
        public int Id { get; set; }
        public int Number { get; set; }

        [Display(Name = "Count of shelves")]
        public int CountOfShelves { get; set; }

        public int ShopId { get; set; }
    }
}
