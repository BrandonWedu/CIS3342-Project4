using Microsoft.AspNetCore.Mvc;
using Project4.Models;

namespace Project4.Controllers
{
	public class DashboardController : Controller
	{
		private PropertyList allProperties = new PropertyList();
		public IActionResult Dashboard()
		{
			ViewBag.AllProperties = allProperties.GetAllProperties();
			return View();
		}

		public IActionResult ApplyFilter()
		{
			return View();
		}

		public IActionResult ClearFilter()
		{
			return View();
		}

		public IActionResult ViewDetail(int propertyID)
		{
			Property currentProperty = allProperties.GetPropertyByPropertyID(propertyID);
			return View(currentProperty);
		}
		public IActionResult RequestShowing(int propertyID)
		{
			Property currentProperty = allProperties.GetPropertyByPropertyID(propertyID);
			return View(currentProperty);
		}
		public IActionResult MakeOffer(int propertyID)
		{
			Property currentProperty = allProperties.GetPropertyByPropertyID(propertyID);
			return View(currentProperty);
		}

		public IActionResult MakeShowingRequest(string message)
		{
			ViewBag.ConfirmationMessage = message;
			return View("Confirmation");
		}
	}
}
