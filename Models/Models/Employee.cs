using Models.Validators;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Employee
    {
        public Employee()
        {
            Subordinates = new List<Employee>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
        public string Name { get; set; }
        public string Surname { get; set; }

        [BornNumber(ErrorMessage = "Invalid Rodne Cislo format.")]
        [Display(Name = "Born Number")]
        public string BornNumber { get; set; }
        
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+\d{3}\s?\d{3}\s?\d{3}\s?\d{3}$", ErrorMessage = "Incorrect phone number format.")]
        public string PhoneNumber {  get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid position.")]
        public int PositionId { get; set; } 

        public Position Position { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid shop.")]
        public int ShopId { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public List<Employee> Subordinates { get; set; }
    }
}
