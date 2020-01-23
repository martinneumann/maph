using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WellApi.Models;

namespace WellApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DBTestController : ControllerBase
    {
        /// <summary>
        /// ReconnectToDb.
        /// </summary>
        [HttpGet]
        [ActionName("ReconnectToDb")]
        public IActionResult ReconnectToDb()
        {
            DB.ReconnectToDb();
            return Ok("reconnect");
        }
        /// <summary>
        /// ExecuteSelectSmallWells.
        /// </summary>
        [HttpGet]
        [ActionName("ExecuteSelectSmallWells")]
        public IActionResult ExecuteSelectSmallWells()
        {
            SmallWell[] smallWells = DB.ExecuteSelectSmallWells();
            return Ok(smallWells);
        }
        /// <summary>
        /// ExecuteSelectWell.
        /// </summary>
        /// <param name="wellId"></param> 
        [HttpGet("{wellId}")]
        [ActionName("ExecuteSelectWell")]
        public IActionResult ExecuteSelectWell(int wellId)
        {
            Well well = DB.ExecuteSelectWell(wellId);
            if (well == null)
                return BadRequest("wellId not found!");
            return Ok(well);
        }

        /// <summary>
        /// ExecuteSelectStatusHistory.
        /// </summary>
        /// <param name="wellId"></param> 
        [HttpGet("{wellId}")]
        [ActionName("ExecuteSelectStatusHistory")]
        public IActionResult ExecuteSelectStatusHistory(int wellId)
        {
            MaintenanceLog[] statusHistory = DB.ExecuteSelectStatusHistory(wellId);
            return Ok(statusHistory);
        }
        /// <summary>
        /// ExecuteSelectWellParts.
        /// </summary>
        /// <param name="wellTypeId"></param> 
        [HttpGet("{wellTypeId}")]
        [ActionName("ExecuteSelectWellParts")]
        public Part[] ExecuteSelectWellParts(int wellTypeId)
        {
            return DB.ExecuteSelectWellParts(wellTypeId);
        }

        /// <summary>
        /// ExecuteInsertParts.
        /// </summary>
        /// <param name="parts"></param> 
        [HttpPost]
        [ActionName("ExecuteInsertParts")]
        public IActionResult ExecuteInsertParts(Part[] parts)
        {
            return Ok(DB.ExecuteInsertParts(parts));
        }
        /// <summary>
        /// ExecuteInsertWellType.
        /// </summary>
        /// <param name="wellType"></param> 
        [HttpPost]
        [ActionName("ExecuteInsertWellType")]
        public IActionResult ExecuteInsertWellType(WellType wellType)
        {
            return Ok(DB.ExecuteInsertWellType(wellType));
        }

        public class InsertWellParts
        {
            public int wellTypeId { get; set; }
            public int[] partId { get; set; }
        }
        /// <summary>
        /// ExecuteInsertWellParts.
        /// </summary>
        /// <param name="insertWellParts"></param> 
        [HttpPost]
        [ActionName("ExecuteInsertWellParts")]
        public IActionResult ExecuteInsertWellParts(InsertWellParts insertWellParts)
        {
            DB.ExecuteInsertWellParts(insertWellParts.wellTypeId, insertWellParts.partId);
            return Ok();
        }
        /// <summary>
        /// ExecuteInsertWell.
        /// </summary>
        /// <param name="well"></param> 
        [HttpPost]
        [ActionName("ExecuteInsertWell")]
        public IActionResult ExecuteInsertWell(Well well)
        {
            return Ok(DB.ExecuteInsertWell(well));
        }

        public class InsertStatusHistory
        {
            public MaintenanceLog[] statusHistory { get; set; }
            public int wellId { get; set; }
        }
        /// <summary>
        /// ExecuteInsertStatusHistory.
        /// </summary>
        /// <param name="insertStatusHistory"></param> 
        [HttpPost]
        [ActionName("ExecuteInsertStatusHistory")]
        public IActionResult ExecuteInsertStatusHistory(InsertStatusHistory insertStatusHistory)
        {
            DB.ExecuteInsertStatusHistory(insertStatusHistory.statusHistory, insertStatusHistory.wellId);
            return Ok();
        }
        /// <summary>
        /// ExecuteUpdatePart.
        /// </summary>
        /// <param name="part"></param> 
        [HttpPost]
        [ActionName("ExecuteUpdatePart")]
        public IActionResult ExecuteUpdatePart(Part part)
        {
            DB.ExecuteUpdatePart(part);
            return Ok();
        }
        /// <summary>
        /// ExecuteUpdateWellType.
        /// </summary>
        /// <param name="wellType"></param> 
        [HttpPost]
        [ActionName("ExecuteUpdateWellType")]
        public IActionResult ExecuteUpdateWellType(WellType wellType)
        {
            DB.ExecuteUpdateWellType(wellType);
            return Ok();
        }
        /// <summary>
        /// ExecuteUpdateFundingInfo.
        /// </summary>
        /// <param name="fundingInfoWithWellId"></param> 
        [HttpPost]
        [ActionName("ExecuteUpdateFundingInfo")]
        public IActionResult ExecuteUpdateFundingInfo(FundingInfoWithWellId fundingInfoWithWellId)
        {
            DB.ExecuteUpdateFundingInfo(fundingInfoWithWellId.FundingInfo, fundingInfoWithWellId.WellId);
            return Ok();
        }
        /// <summary>
        /// ExecuteUpdateLocation.
        /// </summary>
        /// <param name="locationWithWellId"></param> 
        [HttpPost]
        [ActionName("ExecuteUpdateLocation")]
        public IActionResult ExecuteUpdateLocation(LocationWithWellId locationWithWellId)
        {
            DB.ExecuteUpdateLocation(locationWithWellId.Location, locationWithWellId.WellId);
            return Ok();
        }
        /// <summary>
        /// ExecuteUpdateWell.
        /// </summary>
        /// <param name="changedWell"></param> 
        [HttpPost]
        [ActionName("ExecuteUpdateWell")]
        public IActionResult ExecuteUpdateWell(ChangedWell changedWell)
        {
            DB.ExecuteUpdateWell(changedWell);
            return Ok();
        }
        public class UpdateStatusHistory
        {
            public MaintenanceLog statusHistory { get; set; }
            public int wellId { get; set; }
        }
        /// <summary>
        /// ExecuteUpdateStatusHistory.
        /// </summary>
        /// <param name="updateStatusHistory"></param> 
        [HttpPost]
        [ActionName("ExecuteUpdateStatusHistory")]
        public IActionResult ExecuteUpdateStatusHistory(UpdateStatusHistory updateStatusHistory)
        {
            DB.ExecuteUpdateStatusHistory(updateStatusHistory.statusHistory, updateStatusHistory.wellId);
            return Ok();
        }
    }
}