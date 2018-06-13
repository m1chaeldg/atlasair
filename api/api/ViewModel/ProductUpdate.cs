using System.ComponentModel.DataAnnotations;

namespace api.ViewModel
{
    public class ProductUpdate : ProductCreate
    {
        [Required]
        public string Id { get; set; }
    }
}
