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
    public class WellController : ControllerBase
    {

        /// <summary>
        /// Get all wells.
        /// </summary>
        [HttpGet]
        [ActionName("GetAll")]
        public SmallWell[] GetAll()
        {
            return DB.ExecuteSelectSmallWells();
        }

        /// <summary>
        /// Get all nearby wells.
        /// </summary>
        /// <param name="searchNearbyWells"></param> 
        [HttpPost("{searchNearbyWells}")]
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
        public Well GetWell(int wellId)
        {
            // get Image sneeded
            return DB.GetCompleteWell(wellId);
        }

        /// <summary>
        /// Creates a specific well.
        /// </summary>
        /// <param name="well"></param> 
        [HttpPost("{well}")]
        [ActionName("PostNewWell")]
        public IActionResult PostNewWell(Well well)
        {
            // Image creation needed
            if (DB.AddCompleteWell(well))
                return Ok();
            else
                return BadRequest();
        }

        /// <summary>
        /// Updates a specific well.
        /// </summary>
        /// <param name="well"></param> 
        [HttpPost("{well}")]
        [ActionName("PostUpdateWell")]
        public IActionResult PostUpdateWell(Well well)
        {
            DB.UpdateCompleteWell(well);
            return Ok();
        }

        /// <summary>
        /// Deletes a specific well.
        /// </summary>
        /// <param name="Id"></param> 
        [HttpDelete("{Id}")]
        [ActionName("DeleteWell")]
        public IActionResult DeleteWell(int Id)
        {
            DB.ExecuteDeleteWell(Id);
            return Ok();
        }
    }
}