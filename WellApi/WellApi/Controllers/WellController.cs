using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using WellApi.Models;

namespace WellApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WellController : ControllerBase
    {

        /// <summary>
        /// Get all wells.
        /// </summary>
        [HttpGet]
        [ActionName("GetAll")]
        [ProducesResponseType(typeof(SmallWell[]), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult GetAll()
        {
            try
            {
                SmallWell[] smallWells = DB.ExecuteSelectSmallWells();
                if (smallWells == null)
                    return BadRequest("Something went wrong!");
                return Ok(smallWells);
            }
            catch
            {
                return Conflict("Server error!");
            }
        }

        /// <summary>
        /// Get all nearby wells. SearchRadius in meter.
        /// </summary>
        /// <param name="locationForSearch"></param> 
        [HttpPost]
        [ActionName("GetNearbyWells")]
        [ProducesResponseType(typeof(SmallWell[]), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult GetNearbyWells(LocationForSearch locationForSearch)
        {
            try
            {
                SmallWell[] smallWells = DB.ExecuteSelectNearbySmallWells(locationForSearch);
                if (smallWells == null)
                    return BadRequest("Something went wrong!");
                return Ok(smallWells);
            }
            catch
            {
                return Conflict("Server error!");
            }
        }

        /// <summary>
        /// Get a specific well.
        /// </summary>
        /// <param name="wellId"></param> 
        [HttpGet("{wellId}")]
        [ActionName("GetWell")]
        [ProducesResponseType(typeof(Well), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult GetWell(int wellId)
        {
            try
            {
                Well well = DB.GetCompleteWell(wellId);
                if (well == null)
                    return BadRequest("Well with Id not found");
                return Ok(well);
            }
            catch
            {
                return Conflict("Server error!");
            }
        }

        /// <summary>
        /// Creates a specific well.
        /// </summary>
        /// <param name="well"></param> 
        [HttpPost]
        [ActionName("PostNewWell")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult PostNewWell(Well well)
        {
            try
            {
                int wellId = DB.AddCompleteWell(well);
                if (wellId == 0)
                    return BadRequest("Well was not inserted!");
                return Ok(wellId);
            }
            catch
            {
                return Conflict("Server error!");
            }
        }

        /// <summary>
        /// Updates a specific well.
        /// </summary>
        /// <param name="well"></param> 
        [HttpPost]
        [ActionName("PostUpdateWell")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult PostUpdateWell(Well well)
        {
            try
            {
                int affected = DB.UpdateCompleteWell(well);
                if (affected > 0)
                    return Ok(affected);
                return BadRequest("Nothing Updated!");
            }
            catch
            {
                return Conflict("Server error!");
            }
        }

        /// <summary>
        /// Deletes a specific well.
        /// </summary>
        /// <param name="Id"></param> 
        [HttpDelete("{Id}")]
        [ActionName("DeleteWell")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult DeleteWell(int Id)
        {
            try
            {
                if (DB.ExecuteDeleteWell(Id))
                    return Ok(Id);
                return BadRequest("Id not found!");
            }
            catch
            {
                return Conflict("Server error!");
            }
        }
    }
}