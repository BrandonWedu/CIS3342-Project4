using Microsoft.AspNetCore.Mvc;

namespace Project4.Controllers
{
    //Handles Creating an account and logging in
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
