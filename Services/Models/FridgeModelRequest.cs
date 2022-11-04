using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class FridgeModelRequest
    {
        [Required]
        public string Name { get; set; }
        public int Year { get; set; }
    }
}
