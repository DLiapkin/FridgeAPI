using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public int DefaultQuantity { get; set; }

        public virtual ICollection<FridgeProduct> Products { get; set; }
    }
}
