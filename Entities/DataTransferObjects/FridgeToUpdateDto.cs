using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class FridgeToUpdateDto
    {
        [Required(ErrorMessage = "Fridge name id is a required field.")]
        public string Name { get; set; }
        public string OwnerName { get; set; }
        [Required(ErrorMessage = "Fridge ModelId is a required field.")]
        public Guid ModelId { get; set; }
    }
}
