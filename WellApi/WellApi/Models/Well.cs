using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi.Models
{
    public class Well
    {
        public Well()
        {
            Location = new Location();
            FundingInfo = new FundingInfo();
            WellType = new WellType();
        }
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? PredictedMaintenance { get; set; }
        public MaintenanceLog[] MaintenanceLogs { get; set; }
        public Location Location { get; set; }
        public FundingInfo FundingInfo { get; set; }
        public WellType WellType { get; set; }
    }

    public class NewWell
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public Location Location { get; set; }
        public NewFundingInfo FundingInfo { get; set; }
        public int? WellTypeId { get; set; }
    }

    public class ChangedWell
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public Location Location { get; set; }
        public FundingInfo FundingInfo { get; set; }
        public int? WellTypeId { get; set; }
    }

    public class SmallWell
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public Location Location { get; set; }
    }
}
