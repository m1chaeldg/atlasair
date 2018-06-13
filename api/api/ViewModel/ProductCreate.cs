using System.ComponentModel.DataAnnotations;

namespace api.ViewModel
{
    public class ProductCreate
    {
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Year { get; set; }
    }
}
