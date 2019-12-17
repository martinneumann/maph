using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi
{
    public class RepairInstructions
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public Part PartToRepair { get; set; }
        public Instruction[] Instruction { get; set; }
    }

    public class Instruction
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

    }

}
