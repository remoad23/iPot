using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace iPotAPI.Controllers
{
    /**
     * Controller for usual Actions that dont fit into a single Controller
     */
    public class GeneralController : Controller
    {


        public IActionResult GetTime()
        {
            return Ok(DateTime.Now);
        }

    }
}