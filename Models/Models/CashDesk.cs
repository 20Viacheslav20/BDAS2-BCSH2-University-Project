using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class CashDesk
    {
        public int Id { get; set; }

        [Display(Name = "Cash desk number")]
        public int Count { get; set; }

        [Display(Name = "Is self")]
        public bool IsSelf { get; set; }
    }
}
