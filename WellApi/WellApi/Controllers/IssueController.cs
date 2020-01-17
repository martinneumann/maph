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
            return issues.Single(i => i.Id == id);
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
            issues = issues.Where(w => w.Id != issue.Id).ToList();
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
            issues = issues.Where(w => w.Id != id).ToList();
            return Ok();
        }
    }
}