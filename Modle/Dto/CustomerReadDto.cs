using Dto;
using System;

namespace Dto
{
    public class CustomerReadDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public CompanyReadDto Company { get; set; }
    }
}
