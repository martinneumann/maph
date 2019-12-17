using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WellApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private IEnumerable<SmallIssue> smallIssues;
        private IEnumerable<Issue> issues;

        public IssueController()
        {
            issues = Enumerable.Range(0,1).Select(index => new Issue
            {
                ID = 0,
                Description = "First Issue",
                CreationDate = new DateTime(2019,11,10),
                WellID = 1
            });
            smallIssues = Enumerable.Range(0, 1).Select(index => new SmallIssue
            {
                ID = 0,
                CreationDate = new DateTime(2019, 11, 10),
                WellID = 1
            });
        }

        /// <summary>
        /// Get all issues.
        /// </summary>
        [HttpGet]
        [ActionName("GetAll")]
        public SmallIssue[] GetAll()
        {
            return smallIssues.ToArray();
        }

        /// <summary>
        /// Get a specific issue.
        /// </summary>
        /// <param name="id"></param> 
        [HttpGet("{id}")]
        [ActionName("GetIssue")]
        public Issue GetIssue(int id)
        {
            return issues.Single(i => i.ID == id);
        }

        /// <summary>
        /// Creates a specific Issue.
        /// </summary>
        /// <param name="issue"></param> 
        [HttpPost("{issue}")]
        [ActionName("PostNewIssue")]
        public IActionResult PostNewIssue(Issue issue)
        {
            issues.Append(issue);
            return Ok();
        }

        /// <summary>
        /// Updates a specific Issue.
        /// </summary>
        /// <param name="issue"></param> 
        [HttpPost("{issue}")]
        [ActionName("PostUpdateIssue")]
        public IActionResult PostUpdateIssue(Issue issue)
        {
            issues = issues.Where(w => w.ID != issue.ID).ToList();
            issues.Append(issue);
            return Ok();
        }

        /// <summary>
        /// Deletes a specific Issue.
        /// </summary>
        /// <param name="id"></param> 
        [HttpDelete("{id}")]
        [ActionName("DeleteIssue")]
        public IActionResult DeleteIssue(int id)
        {
            issues = issues.Where(w => w.ID != id).ToList();
            return Ok();
        }
    }
}