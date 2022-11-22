using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class FridgeProduct : BaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid FridgeId { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Fridge Fridge { get; set; }
    }
}
