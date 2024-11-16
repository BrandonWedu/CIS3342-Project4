using Microsoft.AspNetCore.Mvc;

namespace Project4.Controllers
{
    //Handles Showing Create and Manage
    public class ShowingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
