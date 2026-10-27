using System;
using System.Collections.Generic;
using System.Text;


namespace DataAccess.Model
{
    public class AmusementPark
    {
        public int ParkId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StreetName { get; set; } = string.Empty;
        public string StreetNumber { get; set; } = string.Empty;

        public PostalCode? PostalCode { get; set; }

        public List<Restaurant> Restaurants { get; set; } = new();
    }
}

