using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Project4.Models;
using System.Text.Json;

namespace Project4.Controllers
{
    //Handles Home Create, Modify and search
    public class RealEstateHomeController : Controller
    {
        [HttpGet]
        public IActionResult CreateHome()
        {
            if (HttpContext.Session.GetString("Agent") == null)
            {
                return View("Dashboard");
            }
            string agentJson = HttpContext.Session.GetString("Agent");
            Agent agent = JsonSerializer.Deserialize<Agent>(agentJson);
            return View(new Home(
                agent,
                0,
                new Address("", "", States.Alabama, ""),
                PropertyType.SingleFamily,
                DateTime.Now.Year,
                GarageType.SingleCar,
                "",
                DateTime.Now, 
                SaleStatus.OffMarket, 
                new Images(),
                new Amenities(), 
                new TemperatureControl(HeatingTypes.CentralHeating, CoolingTypes.CentralAir),
                new Rooms(), 
                new Utilities()
                ));
        }
        [HttpPost]
        public IActionResult CreateHome(Home home)
        {
            if (HttpContext.Session.GetString("Agent") != null)
            {
                return View("Dashboard");
            }
            return View(home);
        }
        [HttpPost]
        public IActionResult AddHome(Home home) {

            return View();
        }
    }
}
