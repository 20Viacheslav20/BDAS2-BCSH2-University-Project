using System.ComponentModel.DataAnnotations;

namespace Models.Models.Categor
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is required.")]
        [MinLength(4, ErrorMessage = "Category must be at least 4 characters.")]
        public string Name { get; set; }

    }
}

