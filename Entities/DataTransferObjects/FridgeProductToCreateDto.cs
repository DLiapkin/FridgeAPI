using System;

namespace Entities.DataTransferObjects
{
    public class FridgeProductToCreateDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
