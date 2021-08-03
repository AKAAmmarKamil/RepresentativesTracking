using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Modle.Model
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }
    }
}
