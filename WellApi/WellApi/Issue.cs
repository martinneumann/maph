using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellApi
{
    public class Issue
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
        public bool Open { get; set; }
        public string ConfirmedBy { get; set; }
        public DateTime SolvedDate { get; set; }
        public string RepairedBy { get; set; }
        public IFormFile Bill { get; set; }
        public bool Works { get; set; }
        public Part[] BrokenParts { get; set; }
        public int WellId { get; set; }
    }

    //
    // For API Controller
    //

    public class SmallIssue
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public int WellId { get; set; }
    }
}
