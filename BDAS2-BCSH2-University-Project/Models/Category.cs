using System.ComponentModel.DataAnnotations;

namespace BDAS2_BCSH2_University_Project.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
