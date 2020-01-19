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
            return DB.GetSmallWells();
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
            return DB.GetSmallWells();
        }

        /// <summary>
        /// Get a specific well.
        /// </summary>
        /// <param name="Id"></param> 
        [HttpGet("{Id}")]
        [ActionName("GetWell")]
        public Well GetWell(int Id)
        {
            // get Image sneeded
            return DB.GetWell(Id);
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
            if (DB.CreateNewWell(well))
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
            DB.UpdateWell(well);
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
            DB.DeleteWell(Id);
            return Ok();
        }
    }
}