using Microsoft.AspNetCore.Mvc;

namespace Project4.Controllers
{
    //Handles Offer Create and Manage
    public class OfferController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
