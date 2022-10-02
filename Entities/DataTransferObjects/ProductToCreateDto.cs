using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class ProductToCreateDto
    {
        [Required(ErrorMessage = "Product name is a required field.")]
        public string Name { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Default quantity can't be less than 0.")]
        public int DefaultQuantity { get; set; }
    }
}
