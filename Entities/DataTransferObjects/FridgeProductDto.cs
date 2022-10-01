using System;

namespace Entities.DataTransferObjects
{
    public class FridgeProductDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
