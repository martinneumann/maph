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
            catch (Exception e)
            {
                return Conflict("Server error! " + e.Message);
            }
        }

        /// <summary>
        /// Get all WellTypes.
        /// </summary>
        [HttpGet]
        [ActionName("GetAllWellTypes")]
        [ProducesResponseType(typeof(WellTypeNoParts[]), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult GetAllWellTypes()
        {
            try
            {
                WellTypeNoParts[] wellTypes = DB.ExecuteSelectWellTypes();
                if (wellTypes == null)
                    return BadRequest("Something went wrong!");
                return Ok(wellTypes);
            }
            catch (Exception e)
            {
                return Conflict("Server error! " + e.Message);
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
            catch (Exception e)
            {
                return Conflict("Server error! " + e.Message);
            }
        }
        /// <summary>
        /// Gets the Repair Help for a Part of a Well.
        /// </summary>
        /// <param name="partId"></param> 
        [HttpGet("{partId}")]
        [ActionName("GetRepairHelp")]
        [ProducesResponseType(typeof(RepairHelpForPart), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult GetRepairHelp(int partId)
        {
            try
            {
                RepairHelpForPart repairHelpForPart = DB.GetRepairHelpForPart(partId);
                if (repairHelpForPart == null || repairHelpForPart.PartToRepair == null || repairHelpForPart.RepairHelps == null)
                    return BadRequest("Repair Help not found");
                return Ok(repairHelpForPart);
            }
            catch (Exception e)
            {
                return Conflict("Server error! " + e.Message);
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
            catch (Exception e)
            {
                return Conflict("Server error! " + e.Message);
            }
        }

        /// <summary>
        /// Creates a specific well.
        /// </summary>
        /// <param name="newWell"></param> 
        [HttpPost]
        [ActionName("CreateWell")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult CreateWell(NewWell newWell)
        {
            try
            {
                int? wellId = DB.AddCompleteWell(newWell);
                if (wellId == null)
                    return BadRequest("Well was not inserted!");
                return Ok(wellId);
            }
            catch (Exception e)
            {
                return Conflict("Server error! " + e.Message);
            }
        }

        /// <summary>
        /// Updates a specific well.
        /// </summary>
        /// <param name="changedWell"></param> 
        [HttpPost]
        [ActionName("UpdateWell")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult UpdateWell(ChangedWell changedWell)
        {
            try
            {
                int affected = DB.UpdateCompleteWell(changedWell);
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
        /// Deletes a specific well.
        /// </summary>
        /// <param name="id"></param> 
        [HttpDelete("{id}")]
        [ActionName("DeleteWell")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public IActionResult DeleteWell(int id)
        {
            try
            {
                int affected = DB.ExecuteDeleteWell(id);
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