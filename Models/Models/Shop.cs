using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Shop
    {
        public int Id { get; set; }

        [Display(Name = "Contact tel")]
        public string Contact { get; set; }  
        public double Square {  get; set; }
        public string StringAddress => Address != null ? $"{Address.City} - {Address.Street}" : "";
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public List<Stand> Stands { get; set; }
        public List<CashDesk> CashDesks { get; set; }
    }
}
