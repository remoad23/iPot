using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using iPotAPI.DataTransferObject;
using iPotAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace iPotAPI.Controllers
{
    /**
     * Controller for every settings interaction
     */
    public class SettingsController : Controller
    {
        private PotContext Context;
        public SettingsController(PotContext context)
        {
            Context = context;
        }
        
        [Route("GetAutomaticWatering")]
        public IActionResult GetAutomaticWatering()
        {
            var automatic = Context.Settings.SingleOrDefault().AutomaticWatering;
            return Ok(automatic);
        }
        
        [Route("GetAutomaticLight")]
        public IActionResult GetAutomaticLight()
        {
            var automatic = Context.Settings.SingleOrDefault().AutomaticLight;
            return Ok(automatic);
        }

        [Route("GetIntervalStart")]
        public IActionResult GetIntervalStart()
        {
            var start = Context.Settings.SingleOrDefault().UptimeStart;
            return Ok(start);
        }
        
        [Route("GetIntervalEnd")]
        public IActionResult GetIntervalEnd()
        {
            var end = Context.Settings.SingleOrDefault().UptimeEnd;
            return Ok(end);
        }
        
        [Route("GetMoistureMinimum")]
        public IActionResult GetMoistureMinimum()
        {
            var minimum = Context.Settings.SingleOrDefault().MoistureMinimum;
            return Ok(minimum);
        }
        
        [Route("GetLightMinimum")]
        public IActionResult GetLightMinimum()
        {
            var minimum = Context.Settings.SingleOrDefault().LightMinimum;
            return Ok(minimum);
        }

        [Route("SetAutomaticWatering")]
        public IActionResult SetAutomaticWatering([FromBody]dynamic toggle)
        {
            string pr = Convert.ToString(toggle);
            var val = JsonSerializer.Deserialize<Settingsbool>(pr);
            
            Context.Settings.SingleOrDefault().AutomaticWatering = val.automaticWatering;
            Context.SaveChanges();
            return Ok();
        }
        
        [Route("SetAutomaticLight")]
        public IActionResult SetAutomaticLight([FromBody]dynamic toggle)
        {
            string pr = Convert.ToString(toggle);
            var val = JsonSerializer.Deserialize<Settingsbool>(pr);
            
            Context.Settings.SingleOrDefault().AutomaticLight = val.automaticLight;
            Context.SaveChanges();
            return Ok();
        }
        
        [Route("SetNotificationLight")]
        public IActionResult SetNotificationLight([FromBody]dynamic toggle)
        {
            string pr = Convert.ToString(toggle);
            var val = JsonSerializer.Deserialize<Settingsbool>(pr);
            
            Context.Settings.SingleOrDefault().NotificationLight = val.NotificationLight;
            Context.SaveChanges();
            return Ok();
        }
        
        [Route("SetNotificationMoisture")]
        public IActionResult SetNotificationMoisture([FromBody]dynamic toggle)
        {
            string pr = Convert.ToString(toggle);
            var val = JsonSerializer.Deserialize<Settingsbool>(pr);
            
            Context.Settings.SingleOrDefault().NotificationMoisture = val.NotificationMoisture;
            Context.SaveChanges();
            return Ok();
        }
        
        [Route("SetNotificationWater")]
        public IActionResult SetNotificationWater([FromBody]dynamic toggle)
        {
           
            string pr = Convert.ToString(toggle);
            var val = JsonSerializer.Deserialize<Settingsbool>(pr);
             Console.WriteLine(val.NotificationWater);
            Context.Settings.SingleOrDefault().NotificationWater = val.NotificationWater;
            Context.SaveChanges();
            return Ok();
        }
        
        [Route("GetSettings")]
        public IActionResult GetSettings()
        {
            var settings = Context.Settings.SingleOrDefault();

            List<bool> settingsList = new List<bool>();
            settingsList.Add(settings.AutomaticWatering);
            settingsList.Add(settings.AutomaticLight);
            settingsList.Add(settings.NotificationWater);
            settingsList.Add(settings.NotificationLight);
            settingsList.Add(settings.NotificationMoisture);
            
            return Json(settingsList);
        }
    }
}