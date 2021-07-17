using System;
using System.Linq;
using System.Text.Json;
using iPotAPI.DataTransferObject;
using iPotAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace iPotAPI.Controllers
{
    public class WaterController : Controller
    {
        private PotContext Context;

        public WaterController(PotContext context)
        {
            Context = context;
        }
        
        [Route("GetMoistureAndWater")]
        public IActionResult GetMoistureAndWater()
        {
            var moistureAndWater = Context.PlantState.SingleOrDefault();
            if (moistureAndWater is not null)
            {
                return Json(new Tuple<float,float>(moistureAndWater.MoistureValue,moistureAndWater.WaterStorage));
            }
            else
            {
                return NotFound();
            }
            
        }
        
        [Route("SetNeedsWater")]
        [HttpPost]
        public IActionResult SetNeedsWater([FromBody]dynamic value)
        {
            string pr = Convert.ToString(value);
            
            var val = JsonSerializer.Deserialize<SetNeedsWater>(pr);
            var plantState = Context.PlantState.SingleOrDefault();
            plantState.NeedsWater = val.NeedsWater;
            Context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("SetMinimalMoisture")]
        public IActionResult SetMinimalMoisture([FromBody]dynamic value)
        {
            string pr = Convert.ToString(value);
            
            var val = JsonSerializer.Deserialize<SetMinimalMoistureData>(pr);
            var settings = Context.Settings.SingleOrDefault();
            settings.MoistureMinimum = (float)val.MinimalMoisture;
            Context.SaveChanges();

          return Ok();
        }

        [Route("GetMoisture")]
        public IActionResult GetMoisture()
        {
            var moistureAndWater = Context.PlantState.SingleOrDefault();
            if (moistureAndWater is not null)
            {
                return Ok(moistureAndWater.MoistureValue);
            }
            else
            {
                return NotFound();
            }
        }
        
        [Route("GetWater")]
        public IActionResult GetWater()
        {
            var moistureAndWater = Context.PlantState.SingleOrDefault();
            if (moistureAndWater is not null)
            {
                return Ok(moistureAndWater.WaterStorage);
            }
            else
            {
                return NotFound();
            }
        }


    }
}