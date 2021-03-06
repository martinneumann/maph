﻿using System;
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
    [ApiExplorerSettings(IgnoreApi=true)]
    public class DBTestController : ControllerBase
    {
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
            MaintenanceLog[] statusHistory = DB.ExecuteSelectMaintenanceLogs(wellId);
            return Ok(statusHistory);
        }
        /// <summary>
        /// ExecuteSelectWellParts.
        /// </summary>
        /// <param name="wellTypeId"></param> 
        [HttpGet("{wellTypeId}")]
        [ActionName("ExecuteSelectWellParts")]
        public PartWithPrediction[] ExecuteSelectWellParts(int wellTypeId)
        {
            return DB.ExecuteSelectWellParts(wellTypeId);
        }

        /// <summary>
        /// ExecuteInsertPart.
        /// </summary>
        /// <param name="newPart"></param> 
        [HttpPost]
        [ActionName("ExecuteInsertPart")]
        public IActionResult ExecuteInsertPart(NewPart newPart)
        {
            return Ok(DB.ExecuteInsertPart(newPart));
        }
        /// <summary>
        /// ExecuteInsertWellType.
        /// </summary>
        /// <param name="newWellType"></param> 
        [HttpPost]
        [ActionName("ExecuteInsertWellType")]
        public IActionResult ExecuteInsertWellType(NewWellType newWellType)
        {
            return Ok(DB.ExecuteInsertWellType(newWellType));
        }


        /// <summary>
        /// ExecuteInsertWellPart.
        /// </summary>
        /// <param name="insertWellPart"></param> 
        [HttpPost]
        [ActionName("ExecuteInsertWellPart")]
        public IActionResult ExecuteInsertWellPart(InsertWellPart insertWellPart)
        {
            DB.ExecuteInsertWellPart(insertWellPart.WellTypeId, insertWellPart.PartId);
            return Ok();
        }
        /// <summary>
        /// ExecuteInsertWell.
        /// </summary>
        /// <param name="newWell"></param> 
        [HttpPost]
        [ActionName("ExecuteInsertWell")]
        public IActionResult ExecuteInsertWell(NewWell newWell)
        {
            return Ok(DB.ExecuteInsertWell(newWell));
        }


        /// <summary>
        /// ExecuteInsertMaintenanceLog.
        /// </summary>
        /// <param name="insertMaintenanceLog"></param> 
        [HttpPost]
        [ActionName("ExecuteInsertMaintenanceLog")]
        public IActionResult ExecuteInsertMaintenanceLog(InsertMaintenanceLog insertMaintenanceLog)
        {
            DB.ExecuteInsertMaintenanceLog(insertMaintenanceLog.MaintenanceLog, insertMaintenanceLog.WellId);
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
        /// <summary>
        /// ExecuteUpdateMaintenanceLog.
        /// </summary>
        /// <param name="updateMaintenanceLog"></param> 
        [HttpPost]
        [ActionName("ExecuteUpdateMaintenanceLog")]
        public IActionResult ExecuteUpdateMaintenanceLog(UpdateMaintenanceLog updateMaintenanceLog)
        {
            DB.ExecuteUpdateMaintenanceLog(updateMaintenanceLog.MaintenanceLog, updateMaintenanceLog.WellId);
            return Ok();
        }
    }
}