using System.ComponentModel.DataAnnotations;

namespace BDAS2_BCSH2_University_Project.Models
{
    public class Employer
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }

        [Required]
        public string BornNumber { get; set; }
        
        [Required]
        public string PhoneNumber {  get; set; }


    }
}
