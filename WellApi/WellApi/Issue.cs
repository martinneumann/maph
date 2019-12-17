using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi
{
    public class Issue
    {
        public int ID { get; set; }
        public int WellID { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public bool Open { get; set; }
        public Part[] BrokenParts { get; set; }
        public string ConfirmedBy { get; set; }
        public DateTime SolvedDate { get; set; }
        public string RepairedBy { get; set; }
        public string Bill { get; set; }
    }

    //
    // For API Controller
    //

    public class SmallIssue
    {
        public int ID { get; set; }
        public DateTime CreationDate { get; set; }
        public int WellID { get; set; }
    }
}
