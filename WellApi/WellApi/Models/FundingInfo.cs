using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi.Models
{
    public class FundingInfo
    {
        public string Organisation { get; set; }
        public DateTime OpeningDate { get; set; }
        public double Price { get; set; }
    }

    public class NewFundingInfo
    {
        public string Organisation { get; set; }
        public double Price { get; set; }
    }

    public class FundingInfoWithWellId
    {
        public FundingInfo FundingInfo { get; set; }
        public int WellId { get; set; }
    }
}
