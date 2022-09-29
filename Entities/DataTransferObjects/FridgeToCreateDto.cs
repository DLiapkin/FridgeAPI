using System;

namespace Entities.DataTransferObjects
{
    public class FridgeToCreateDto
    {
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public Guid ModelId { get; set; }
    }
}
