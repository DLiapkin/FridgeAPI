using System;

namespace Services.Models
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DefaultQuantity { get; set; }
    }
}
