using System;

namespace Services.Models
{
    public class FridgeResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public Guid ModelId { get; set; }
    }
}
