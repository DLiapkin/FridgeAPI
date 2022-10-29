using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class FridgeModel : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public int Year { get; set; }

        public virtual ICollection<Fridge> Fridges { get; set; }
    }
}
