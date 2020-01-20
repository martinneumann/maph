using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System.Net;
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
        public IActionResult GetAll()
        {
            SmallWell[] smallWells = DB.ExecuteSelectSmallWells();
            if (smallWells == null)
                return BadRequest("Something went wrong!");
            return Ok(smallWells);
        }

        /// <summary>
        /// Get all nearby wells.
        /// </summary>
        /// <param name="searchNearbyWells"></param> 
        [HttpPost]
        [ActionName("GetNearbyWells")]
        public SmallWell[] GetNearbyWells(SearchNearbyWells searchNearbyWells)
        {
            //Berechnung fehlt
            return DB.ExecuteSelectSmallWells();
        }

        /// <summary>
        /// Get a specific well.
        /// </summary>
        /// <param name="wellId"></param> 
        [HttpGet("{wellId}")]
        [ActionName("GetWell")]
        [ProducesResponseType(typeof(Well), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult GetWell(int wellId)
        {
            // get Image sneeded
            Well well = DB.GetCompleteWell(wellId);
            if (well == null)
                return BadRequest("Well with Id not found");
            return Ok(well);
        }

        /// <summary>
        /// Creates a specific well.
        /// </summary>
        /// <param name="well"></param> 
        [HttpPost]
        [ActionName("PostNewWell")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult PostNewWell(Well well)
        {
            // Image creation needed
            int wellId = DB.AddCompleteWell(well);
            if (wellId == 0)
                return BadRequest("Well was not inserted!");
            return Ok(wellId);
        }

        /// <summary>
        /// Updates a specific well.
        /// </summary>
        /// <param name="well"></param> 
        [HttpPost]
        [ActionName("PostUpdateWell")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult PostUpdateWell(Well well)
        {
            int affected = DB.UpdateCompleteWell(well);
            if (affected > 0)
                return Ok(affected);
            return BadRequest("Nothing Updated!");
        }

        /// <summary>
        /// Deletes a specific well.
        /// </summary>
        /// <param name="Id"></param> 
        [HttpDelete("{Id}")]
        [ActionName("DeleteWell")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult DeleteWell(int Id)
        {
            if (DB.ExecuteDeleteWell(Id))
                return Ok(Id);
            return BadRequest("Id not found!");
        }
    }
}