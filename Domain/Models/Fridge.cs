using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Fridge : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string OwnerName { get; set; }
        [Required]
        public Guid ModelId { get; set; }

        public virtual FridgeModel Model { get; set; }
        public virtual ICollection<FridgeProduct> Products { get; set; }
    }
}
