using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class FridgeProduct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid FridgeId { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Fridge Fridge { get; set; }
    }
}
