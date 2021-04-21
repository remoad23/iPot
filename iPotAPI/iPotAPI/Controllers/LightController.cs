using Microsoft.AspNetCore.Mvc;

namespace iPotAPI.Controllers
{
    /**
     * Controller for every light interaction
     */
    public class LightController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}