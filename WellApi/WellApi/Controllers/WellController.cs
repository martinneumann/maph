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
        private IEnumerable<SmallWell> smallWells;
        private IEnumerable<Well> wells;
        public WellController()
        {
            var rng = new Random();
            wells = Enumerable.Range(0, 5).Select(index => new Well
            {
                ID = index,
                Name = $"Well {index}",
                Status = "green",
                Location = new Location
                {
                    Longitude = rng.Next(-180,180) + rng.NextDouble(),
                    Latitude = rng.Next(-90, 90) + rng.NextDouble()
                },
                FundingInfo = new FundingInfo
                {
                    Organisation = "ABC",
                    Opening = new DateTime(2019, rng.Next(1, 12), 10),
                    Price = 1000000 + rng.Next(-400000,400000)
                },
                WellType = new WellType
                {
                    Name = "Type X",
                    Particularity = "None",
                    Depth = 100 + rng.Next(-20,20)
                }
            });
            Well[] wellArray = wells.ToArray();
            smallWells = Enumerable.Range(0, 5).Select(index => new SmallWell
            {
                ID = index,
                Name = $"Well {index}",
                Status = "#00FF00",
                Location = wellArray[index].Location
            });
        }

        /// <summary>
        /// Get all wells.
        /// </summary>
        [HttpGet]
        [ActionName("GetAll")]
        public SmallWell[] GetAll()
        {
            return smallWells.ToArray();
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
            SmallWell[] smalls = new SmallWell[3];
            Array.Copy(smallWells.ToArray(), 1, smalls, 0, 3);
            return smalls;
        }

        /// <summary>
        /// Get a specific well.
        /// </summary>
        /// <param name="id"></param> 
        [HttpGet("{id}")]
        [ActionName("GetWell")]
        public Well GetWell(int id)
        {
            return wells.Single(well => well.ID == id);
        }

        /// <summary>
        /// Creates a specific well.
        /// </summary>
        /// <param name="well"></param> 
        [HttpPost("{well}")]
        [ActionName("PostNewWell")]
        public IActionResult PostNewWell(Well well)
        {
            wells.Append(well);
            return Ok();
        }

        /// <summary>
        /// Updates a specific well.
        /// </summary>
        /// <param name="well"></param> 
        [HttpPost("{well}")]
        [ActionName("PostUpdateWell")]
        public IActionResult PostUpdateWell(Well well)
        {
            wells = wells.Where(w => w.ID != well.ID).ToList();
            wells.Append(well);
            return Ok();
        }

        /// <summary>
        /// Deletes a specific well.
        /// </summary>
        /// <param name="id"></param> 
        [HttpDelete("{id}")]
        [ActionName("DeleteWell")]
        public IActionResult DeleteWell(int id)
        {
            wells = wells.Where(w => w.ID != id).ToList();
            return Ok();
        }
    }
}