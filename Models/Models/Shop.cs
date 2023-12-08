using Models.Models.CashDesks;
using Models.Models.Stands;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Shop
    {
        public Shop()
        {
            Stands = new List<ShopStand>();
            CashDesks = new List<ShopCashDesk>();
        }

        public int Id { get; set; }

        [Display(Name = "Contact tel")]
        [Required(ErrorMessage = "Contact tel is required.")]
        [RegularExpression(@"^\+\d{3}\s?\d{3}\s?\d{3}\s?\d{3}$", ErrorMessage = "Incorrect phone number format.")]
        public string Contact { get; set; }

        [Range(10, int.MaxValue, ErrorMessage = "The square must be a non-negative number and more than 10.")]
        public int Square {  get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string StringAddress => Address != null ? $"{Address.City} - {Address.Street}" : "";
        public List<ShopStand> Stands { get; set; }
        public List<ShopCashDesk> CashDesks { get; set; }
    }
}

