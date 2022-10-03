using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class FridgeModelToCreateDto
    {
        [Required]
        public string Name { get; set; }
        public int Year { get; set; }
    }
}
