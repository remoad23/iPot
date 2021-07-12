using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using iPotAPI.DataTransferObject;
using iPotAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace iPotAPI.Controllers
{
    /**
     * Controller for every water interaction
     */
    public class PourTimeController : Controller
    {
        private PotContext Context;

        public PourTimeController(PotContext context)
        {
            Context = context;
        }

        /*
        
        [HttpGet]
        public IActionResult Index()
        {
            
            var pourTime = Context.PourTimes;

            if (pourTime.Any())
            {
                return  Json(pourTime, new JsonSerializerOptions());
            }
            else
            {
                return NotFound();
            }
        }
        
        [HttpGet]
        public IActionResult Show()
        {
            return null;
        }
        
        [Route("CreateTimeToPour")]
        [HttpPost]
        public IActionResult CreateTimeToPour([FromBody] dynamic value)
        {
            string pr = Convert.ToString(value);
            PourTimeData pourtimeData = JsonSerializer.Deserialize<PourTimeData>(pr);

            PourTime pourtime = new PourTime
            {
                percentageToPour = pourtimeData.Percentage,
                timeToPour = pourtimeData.Time
            };
            Context.PourTimes.Add(pourtime);
            Context.SaveChanges(); 
            return Ok("swag");
        } */
    }
}