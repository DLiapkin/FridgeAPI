using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class FridgeProductToCreateDto
    {
        [Required(ErrorMessage = "Product id is a required field.")]
        public Guid ProductId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity can't be less than 0.")]
        public int Quantity { get; set; }
    }
}
