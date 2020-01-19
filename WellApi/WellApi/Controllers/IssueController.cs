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
            return DB.GetSmallIssues();
        }

        /// <summary>
        /// Get a specific issue.
        /// </summary>
        /// <param name="id"></param> 
        [HttpGet("{id}")]
        [ActionName("GetIssue")]
        public Issue GetIssue(int id)
        {
            return DB.GetIssue(id);
        }

        /// <summary>
        /// Creates a specific Issue.
        /// </summary>
        /// <param name="issue"></param> 
        [HttpPost("{issue}")]
        [ActionName("PostNewIssue")]
        public IActionResult PostNewIssue(Issue issue)
        {
            if (DB.NewIssue(issue))
                return Ok();
            else
                return BadRequest();
        }

        /// <summary>
        /// Updates a specific Issue.
        /// </summary>
        /// <param name="issue"></param> 
        [HttpPost("{issue}")]
        [ActionName("PostUpdateIssue")]
        public IActionResult PostUpdateIssue(Issue issue)
        {
            DB.UpdateIssue(issue);
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
            DB.DeleteIssue(id);
            return Ok();
        }
    }
}