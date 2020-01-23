using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi.Models
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


    public class SmallWell
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public Location Location { get; set; }
    }
}
