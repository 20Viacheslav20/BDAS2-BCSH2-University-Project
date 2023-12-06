using System.ComponentModel.DataAnnotations;

namespace Models.Models.Category
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Name { get; set; }

    }
}

