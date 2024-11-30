using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
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
            //string agentJson = HttpContext.Session.GetString("Agent");
            //Agent agent = JsonSerializer.Deserialize<Agent>(agentJson);
            //Home home = new Home(
                //agent,
                //0,
                //new Address("", "", States.Alabama, ""),
                //PropertyType.SingleFamily,
                //DateTime.Now.Year,
                //GarageType.SingleCar,
                //"",
                //DateTime.Now, 
                //SaleStatus.OffMarket, 
                //new Images(),
                //new Amenities(), 
                //new TemperatureControl(HeatingTypes.CentralHeating, CoolingTypes.CentralAir),
                //new Rooms(), 
                //new Utilities()
                //);
            return View();
        }
       
        [HttpPost]
        public IActionResult HomeForm(string button)
        {
            switch (button)
            {
                case "AddRoom":
                    AddRoom();
                    break;
            }
            return View("CreateHome");
        }
        public void AddRoom() 
        {
            if (TempData["RoomCount"] != null)
            {
                TempData["RoomCount"] = (int)TempData["RoomCount"] + 1;
            } 
            else
            {
                TempData["RoomCount"] = 1;
            }
            //save request.form to temp data
            foreach (string key in Request.Form.Keys)
            {
                    TempData[key] = Request.Form[key];
            }
            TempData.Keep();
        }
    }
}
