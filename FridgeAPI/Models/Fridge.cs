using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FridgeAPI.Models
{
    public class Fridge
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string OwnerName { get; set; }
        [Required]
        public Guid ModelId { get; set; }

        public virtual FridgeModel Model { get; set; }
        public virtual ICollection<FridgeProduct> Products { get; set; }
    }
}
