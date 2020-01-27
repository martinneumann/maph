using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi.Models
{
    public class Location
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
    public class LocationWithWellId
    {
        public Location Location { get; set; }
        public int? WellId { get; set; }
    }

    public class LocationForSearch
    {
        public double? SearchRadius { get; set; }
        public Location Location { get; set; }
    }
}
