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
        /// <summary>
        /// Get all issues.
        /// </summary>
        [HttpGet]
        [ActionName("GetAll")]
        public SmallIssue[] GetAll()
        {
            return DB.ExecuteSelectSmallIssues();
        }

        /// <summary>
        /// Get a specific issue.
        /// </summary>
        /// <param name="id"></param> 
        [HttpGet("{id}")]
        [ActionName("GetIssue")]
        public Issue GetIssue(int id)
        {
            return DB.GetCompleteIssue(id);
        }

        /// <summary>
        /// Creates a specific Issue.
        /// </summary>
        /// <param name="issue"></param> 
        [HttpPost("{issue}")]
        [ActionName("PostNewIssue")]
        public IActionResult PostNewIssue(Issue issue)
        {
            DB.AddCompleteNewIssue(issue);
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
            DB.ExecuteUpdateIssue(issue);
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
            DB.ExecuteDeleteIssue(id);
            return Ok();
        }
    }
}