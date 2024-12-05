using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Project4.Controllers
{
	public class SharedController : Controller
	{
		public IActionResult SharedConfirmation()
		{
			List<string> message = JsonConvert.DeserializeObject<List<string>>(TempData["Message"].ToString());
			string action = TempData["Action"].ToString();
			string controller = TempData["Controller"].ToString();
			Console.WriteLine("Actoin:" + action);
			Console.WriteLine("Controller: " + action);
			ViewBag.Message = message;
			ViewBag.Action = action;
			ViewBag.Controller = action;
			return View("~/Views/Shared/Confirmation.cshtml");
		}
	}
}
