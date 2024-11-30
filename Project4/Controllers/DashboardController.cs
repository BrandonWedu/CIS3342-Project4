using Microsoft.AspNetCore.Mvc;
using Project4.Models;

namespace Project4.Controllers
{
    public class DashboardController : Controller
    {
        //private PropertyList allProperties = new PropertyList();
        public IActionResult Dashboard()
        {
            //ViewBag.AllProperties = allProperties.GetAllProperties();
            return View();
        }

        public IActionResult ApplyFilter(string txtFilterCity)
        {
            return View();
        }

        public IActionResult ClearFilter()
        {
            return View();
        }

        public IActionResult ViewDetail(int propertyID)
        {
            //Property currentProperty = allProperties.GetPropertyByPropertyID(propertyID);
            return View();
        }
    }
}
