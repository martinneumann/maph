using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi.Models
{
    public class Part
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class PartWithPrediction
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? PredictedMaintenance { get; set; }
    }
    public class NewPart
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class InsertWellPart
    {
        public int? WellTypeId { get; set; }
        public int? PartId { get; set; }
    }
}
