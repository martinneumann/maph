using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellApi.Models;

namespace WellApi
{
    public class RepairHelp
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public int? Number { get; set; }
    }

    public class RepairHelpForPart
    {
        public RepairHelp[] RepairHelps { get; set; }
        public Part PartToRepair { get; set; }
    }
}
