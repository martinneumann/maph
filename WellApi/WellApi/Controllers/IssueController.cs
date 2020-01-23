using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(typeof(SmallIssue[]), 200)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(DB.ExecuteSelectSmallIssues());
            }
            catch
            {
                return Conflict("Server error!");
            }
        }

        /// <summary>
        /// Get a specific issue.
        /// </summary>
        /// <param name="id"></param> 
        [HttpGet("{id}")]
        [ActionName("GetIssue")]
        [ProducesResponseType(typeof(Issue), 200)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult GetIssue(int id)
        {
            try
            {
                Issue issue = DB.GetCompleteIssue(id);
                if (issue.Id == 0)
                    return BadRequest("Id not found!");
                return Ok(issue);
            }
            catch
            {
                return Conflict("Server error!");
            }
        }

        /// <summary>
        /// Creates a specific Issue.
        /// </summary>
        /// <param name="issue"></param> 
        [HttpPost]
        [ActionName("PostNewIssue")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult PostNewIssue(Issue issue)
        {
            try
            {
                if (DB.AddCompleteNewIssue(issue))
                    return Ok("Created");
                return BadRequest("something went wrong");
            }
            catch
            {
                return Conflict("Server error!");
            }
        }

        /// <summary>
        /// Updates a specific Issue.
        /// </summary>
        /// <param name="issue"></param> 
        [HttpPost]
        [ActionName("PostUpdateIssue")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult PostUpdateIssue(Issue issue)
        {
            try
            {
                DB.UpdateCompleteIssue(issue);
                return Ok("updated");
            }
            catch
            {
                return Conflict("Server error!");
            }
        }

        /// <summary>
        /// Deletes a specific Issue.
        /// </summary>
        /// <param name="id"></param> 
        [HttpDelete("{id}")]
        [ActionName("DeleteIssue")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult DeleteIssue(int id)
        {
            try
            {
                DB.ExecuteDeleteIssue(id);
                return Ok("Deleted");
            }
            catch
            {
                return Conflict("Server error!");
            }
        }
    }
}