using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi
{
    public class Issue
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public int WellID { get; set; }
        public string ConfirmedBy { get; set; }
        public Part[] BrokenParts { get; set; }
        public string RepairedBy { get; set; }
        public string Bill { get; set; }
    }


    public class SmallIssue
    {
        public int ID { get; set; }
        public DateTime CreationDate { get; set; }
        public int WellID { get; set; }
    }
}
