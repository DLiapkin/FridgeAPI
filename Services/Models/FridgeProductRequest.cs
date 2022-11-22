using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class FridgeProductRequest
    {
        [Required(ErrorMessage = "Product id is a required field.")]
        public Guid ProductId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity can't be less than 0.")]
        public int Quantity { get; set; }
    }
}
