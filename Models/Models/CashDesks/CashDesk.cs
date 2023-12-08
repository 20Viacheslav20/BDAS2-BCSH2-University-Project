using System.ComponentModel.DataAnnotations;

namespace Models.Models.CashDesks
{
    public class CashDesk
    {
        public int Id { get; set; }

        [Display(Name = "Cash desk number")]
        [Range(1, int.MaxValue, ErrorMessage = "Please write a valid number.")]
        public int Number { get; set; }

        [Display(Name = "Is self")]
        public bool IsSelf { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid shop.")]
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}
