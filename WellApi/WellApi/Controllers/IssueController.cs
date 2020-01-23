﻿using System;
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
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult GetAll()
        {
            try
            {
                SmallIssue[] smallIssues = DB.ExecuteSelectSmallIssues();
                if (smallIssues == null)
                    return BadRequest("Something went wrong!");
                return Ok(smallIssues);
            }
            catch (Exception e)
            {
                return Conflict("Server error! " + e.Message);
            }
        }

        /// <summary>
        /// Get a specific issue.
        /// </summary>
        /// <param name="id"></param> 
        [HttpGet("{id}")]
        [ActionName("GetIssue")]
        [ProducesResponseType(typeof(Issue), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult GetIssue(int id)
        {
            try
            {
                Issue issue = DB.GetCompleteIssue(id);
                if (issue == null || issue.Id == 0)
                    return BadRequest("Id not found!");
                return Ok(issue);
            }
            catch (Exception e)
            {
                return Conflict("Server error! " + e.Message);
            }
        }

        /// <summary>
        /// Creates a specific Issue.
        /// </summary>
        /// <param name="newIssue"></param> 
        [HttpPost]
        [ActionName("PostNewIssue")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult PostNewIssue(NewIssue newIssue)
        {
            try
            {
                if (DB.AddCompleteNewIssue(newIssue))
                    return Ok("Created");
                return BadRequest("something went wrong");
            }
            catch (Exception e)
            {
                return Conflict("Server error! " + e.Message);
            }
        }

        /// <summary>
        /// Updates a specific Issue.
        /// </summary>
        /// <param name="issue"></param> 
        [HttpPost]
        [ActionName("PostUpdateIssue")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult PostUpdateIssue(Issue issue)
        {
            try
            {
                int affected = DB.UpdateCompleteIssue(issue);
                if (affected > 0)
                    return Ok(affected);
                return BadRequest("Nothing Updated!");
            }
            catch (Exception e)
            {
                return Conflict("Server error! " + e.Message);
            }
        }

        /// <summary>
        /// Deletes a specific Issue.
        /// </summary>
        /// <param name="id"></param> 
        [HttpDelete("{id}")]
        [ActionName("DeleteIssue")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult DeleteIssue(int id)
        {
            try
            {
                int affected = DB.ExecuteDeleteIssue(id);
                if (affected > 0)
                    return Ok(id);
                return BadRequest("Nothing Deleted!");
            }
            catch (Exception e)
            {
                return Conflict("Server error! " + e.Message);
            }
        }
    }
}