using Dto;
namespace Dto
{
    public class CustomerReadDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public CompanyReadDto Company { get; set; }
    }
}
