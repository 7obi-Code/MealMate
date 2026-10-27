using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.Model
{
    public class Price
    {
        public int PriceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ValidFrom { get; set; }
    }
}
