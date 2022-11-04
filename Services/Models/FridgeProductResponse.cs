using System;

namespace Services.Models
{
    public class FridgeProductResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
