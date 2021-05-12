using System;
using System.Collections.Generic;

namespace Dto
{
    public class UserReadDto
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public int Type { get; set; }
        public string CompanyName { get; set; }
    }
}
