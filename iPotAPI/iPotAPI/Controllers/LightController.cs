using System;
using System.Linq;
using System.Text.Json;
using iPotAPI.DataTransferObject;
using iPotAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace iPotAPI.Controllers
{
    /**
     * Controller for every light interaction
     */
    public class LightController : Controller
    {

        private PotContext Context;

        public LightController(PotContext context)
        {
            Context = context;
        }

        public IActionResult GetLightIntensity()
        {
            var intensity = Context.PlantState.SingleOrDefault().LedIntensity;
            return Json(intensity);
        }

        [Route("GetLightIntensityRaw")]
        public IActionResult GetLightIntensityRaw()
        {
            var intensity = Context.PlantState.SingleOrDefault().LedIntensity;
            return Ok(intensity);
        }
        

        [Route("GetMinimumLight")]
        public IActionResult GetMinimumLight()
        {
            var intensity = Context.Settings.SingleOrDefault();
            return Json(intensity.LightMinimum);
        }
        
        [HttpPost]
        [Route("SetLightMinimum")]
        public IActionResult SetLightMinimum([FromBody] dynamic intensity)
        {
            string pr = Convert.ToString(intensity);
            var val = JsonSerializer.Deserialize<SetLightIntensity>(pr);
            
            Console.WriteLine(val.intensity);
            var plantState = Context.Settings.SingleOrDefault();
            if (plantState is not null)
            {
                plantState.LightMinimum = (float)val.intensity;
                Context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        
        [Route("GetLightAndAmbient")]
        public IActionResult GetLightAndAmbient()
        {
            var intensity = Context.PlantState.SingleOrDefault();
            return Json(new Tuple<float,float>(intensity.LedIntensity,intensity.AmbientLightIntensity));
        }

        [HttpPost]
        [Route("SetLightIntensity")]
        public IActionResult SetLightIntensity([FromBody] dynamic intensity)
        {
            string pr = Convert.ToString(intensity);
            var val = JsonSerializer.Deserialize<SetLightIntensity>(pr);
            
            Console.WriteLine(val.intensity);
            var plantState = Context.PlantState.SingleOrDefault();
            if (plantState is not null)
            {
                plantState.LedIntensity = (float)val.intensity;
                Context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        
        
    }
}