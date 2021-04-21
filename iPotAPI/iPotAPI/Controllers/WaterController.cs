using Microsoft.AspNetCore.Mvc;

namespace iPotAPI.Controllers
{
    /**
     * Controller for every water interaction
     */
    public class WaterController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}