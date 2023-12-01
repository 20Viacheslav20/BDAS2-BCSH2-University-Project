﻿using System.ComponentModel.DataAnnotations;

namespace BDAS2_BCSH2_University_Project.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Born Number")]
        public string BornNumber { get; set; }
        
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber {  get; set; }

        public int PositionId { get; set; } 

        public Position Position { get; set; }
    }
}
