using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellApi.Models;

namespace WellApi
{
    public class Well
    {
        public int Id { get; set; }
        public string Name { get; set; }
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

    

}
