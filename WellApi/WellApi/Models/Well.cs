using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi.Models
{
    public class Well
    {
        public Well() { }
        public Well(NewWell newWell)
        {
            Name = newWell.Name;
            Status = newWell.Status;
            Location = newWell.Location;
            FundingInfo = newWell.FundingInfo;
            WellType = newWell.WellType;
        }
        public Well(ChangedWell changedWell)
        {
            Id = changedWell.Id;
            Name = changedWell.Name;
            Status = changedWell.Status;
            Location = changedWell.Location;
            FundingInfo = changedWell.FundingInfo;
            WellType = changedWell.WellType;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public MaintenanceLog[] StatusHistory { get; set; }
        public Location Location { get; set; }
        public FundingInfo FundingInfo { get; set; }
        public WellType WellType { get; set; }
    }

    public class NewWell
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public Location Location { get; set; }
        public FundingInfo FundingInfo { get; set; }
        public WellType WellType { get; set; }
    }

    public class ChangedWell
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
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
