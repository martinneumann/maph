using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellApi.Models;

namespace WellApi
{
    public class RepairInstructions
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public Part PartToRepair { get; set; }
        public Instruction[] Instructions { get; set; }
    }

    public class Instruction
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Number { get; set; }
        public string Image { get; set; }

    }

}
