using Microsoft.AspNetCore.Mvc;

namespace Project4.Controllers
{
	public class DashboardController : Controller
	{
		public IActionResult Dashboard()
		{
			return View();
		}
	}
}
