using System;

namespace Dto
{
    public class CustomerForOrderReadDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Company { get; set; }
    }
}
