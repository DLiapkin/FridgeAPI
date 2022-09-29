using System;

namespace Entities.DataTransferObjects
{
    public class FridgeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public Guid ModelId { get; set; }
    }
}
