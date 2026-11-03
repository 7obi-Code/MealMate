using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Model
{
    public class PostalCode
    {
        public int PostalCodeId { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

    }
}
