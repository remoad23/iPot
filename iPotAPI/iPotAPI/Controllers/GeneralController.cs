using Microsoft.AspNetCore.Mvc;

namespace iPotAPI.Controllers
{
    /**
     * Controller for usual Actions that dont fit into a single Controller
     */
    public class GeneralController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}