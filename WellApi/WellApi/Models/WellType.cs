using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi.Models
{
    public class WellType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Part[] Parts { get; set; }
        public string Particularity { get; set; }
        public double Depth { get; set; }
    }

    public class WellTypeNoParts
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
