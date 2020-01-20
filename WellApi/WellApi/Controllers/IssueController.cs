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
        public IActionResult GetIssue(int id)
        {
            Issue issue = DB.GetCompleteIssue(id);
            if (issue.Id == 0)
                return BadRequest("Id not found!");
            return Ok(issue);
        }

        /// <summary>
        /// Creates a specific Issue.
        /// </summary>
        /// <param name="issue"></param> 
        [HttpPost]
        [ActionName("PostNewIssue")]
        [ProducesResponseType(typeof(Well), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult PostNewIssue(Issue issue)
        {
            if(DB.AddCompleteNewIssue(issue))
                return Ok("Created");
            return BadRequest("something went wrong");
        }

        /// <summary>
        /// Updates a specific Issue.
        /// </summary>
        /// <param name="issue"></param> 
        [HttpPost]
        [ActionName("PostUpdateIssue")]
        public IActionResult PostUpdateIssue(Issue issue)
        {
            DB.UpdateCompleteIssue(issue);
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