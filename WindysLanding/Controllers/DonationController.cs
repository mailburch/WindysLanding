using Microsoft.AspNetCore.Mvc;

namespace WindysLanding.Controllers
{
    public class DonationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}