using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi
{
    public class Well
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Status { get; set; }
        public WellStatus[] StatusHistory { get; set; }
        public Location Location { get; set; }
        public FundingInfo FundingInfo { get; set; }
        public WellType WellType { get; set; }
    }

    public class WellStatus
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Works { get; set; }
        public bool Confirmed { get; set; }
        public DateTime StatusChangedDate { get; set; }
    }

    public class Location
    {
        public int Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class FundingInfo
    {
        public int Id { get; set; }
        public string Organisation { get; set; }
        public DateTime OpeningDate { get; set; }
        public double Price { get; set; }
    }

    public class WellType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Part[] Parts { get; set; }
        public string Particularity { get; set; }
        public double Depth { get; set; }
    }

    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    //
    // For API Controller
    //

    public class SmallWell
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public Location Location { get; set; }
    }

    public class SearchNearbyWells
    {
        public double SearchRadius { get; set; }
        public Location Location { get; set; }
    }
}
