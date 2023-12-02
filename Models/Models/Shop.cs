﻿using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Shop
    {
        public int Id { get; set; }

        [Display(Name = "Contact tel")]
        public string Contact { get; set; }
        
        public double Square {  get; set; }

        public Address Address { get; set; }
    }
}
