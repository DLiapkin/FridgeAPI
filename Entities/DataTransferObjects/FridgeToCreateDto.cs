using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class FridgeToCreateDto
    {
        [Required(ErrorMessage = "Fridge name is a required field.")]
        public string Name { get; set; }
        public string OwnerName { get; set; }
        [Required(ErrorMessage = "Fidge modelId is a required field.")]
        public Guid ModelId { get; set; }
    }
}
