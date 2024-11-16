using Microsoft.AspNetCore.Mvc;

namespace Project4.Controllers
{
    //Handles Home Create, Modify and search
    public class RealEstateHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
