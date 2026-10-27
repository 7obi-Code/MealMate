using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public PostalCode? PostalCode { get; set; }
    }
}
