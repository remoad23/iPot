using Microsoft.AspNetCore.Mvc;

namespace iPotAPI.Controllers
{
    /**
     * Controller for every settings interaction
     */
    public class SettingsController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}