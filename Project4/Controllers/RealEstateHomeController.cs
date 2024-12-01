using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
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
            /*
            if (HttpContext.Session.GetString("Agent") == null)
            {
                return View("Dashboard");
            }
            string agentJson = HttpContext.Session.GetString("Agent");
            Agent agent = JsonSerializer.Deserialize<Agent>(agentJson);
            Home home = new Home(
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
                );
            //TempData["Home"] = home;
            return View(home);
            */
            return View();
        }
        [HttpPost]
        public IActionResult CreateHome(Home home)
        {
            if (HttpContext.Session.GetString("Agent") != null)
            {
                return View("Dashboard");
            }
            TempData["Home"] = home;   
            return View(home);
        }
        [HttpPost]
        public IActionResult AddHome(Home home) {

            return View();
        }





        [HttpGet]
        public IActionResult UpdateHome(int homeID)
        {
            //This needs to be an API call
            Home home = ReadHome.GetHomeByHomeID(homeID);

            List<Room> rooms = home.Rooms.List;
            List<Image> images = home.Images.List;
            List<Utility> utilities = home.Utilities.List;
            List<Amenity> amenities = home.Amenities.List;

            string seralizedRooms = JsonConvert.SerializeObject(rooms);
            string seralizedImages = JsonConvert.SerializeObject(images);
            string seralizedUtilites = JsonConvert.SerializeObject(utilities);
            string seralizedAmenities = JsonConvert.SerializeObject(amenities);
            string seralizedHome = JsonConvert.SerializeObject(home);

            HttpContext.Session.SetString("UpdateRooms", seralizedRooms);
            HttpContext.Session.SetString("UpdateImages", seralizedImages);
            HttpContext.Session.SetString("UpdateUtilites", seralizedUtilites);
            HttpContext.Session.SetString("UpdateAmenities", seralizedAmenities);

            ViewBag.Rooms = seralizedRooms;
            ViewBag.Images = seralizedImages;
            ViewBag.Utilites = seralizedUtilites;
            ViewBag.Amenities = seralizedAmenities;
            ViewBag.Home = seralizedHome;
            return View();
        }
        [HttpPost]
        public IActionResult UpdateHome()
        {
            return View("AllListings");
        }

        [HttpPost]
        public IActionResult AddRoom(string width, string length, string roomType)
        {
            string seralizedRooms = HttpContext.Session.GetString("UpdateRooms");
            List<Room> rooms = JsonConvert.DeserializeObject<List<Room>>(seralizedRooms);
            Room newRoom = new Room(0, Enum.Parse<RoomType>(roomType), int.Parse(length), int.Parse(width));
            rooms.Add(newRoom);
            seralizedRooms = JsonConvert.SerializeObject(rooms);
            HttpContext.Session.SetString("UpdateRooms", seralizedRooms);
            return View("UpdateHome");
        }

        [HttpPost]
        public IActionResult RemoveRoom(int roomID, string width, string length, string roomType) 
        {
            string seralizedRooms = HttpContext.Session.GetString("UpdateRooms");
            List<Room> rooms = JsonConvert.DeserializeObject<List<Room>>(seralizedRooms);

            if (roomID == 0)
            {
                rooms.RemoveAll(room =>
                room.Type == Enum.Parse<RoomType>(roomType) &&
                room.Height == int.Parse(length) &&
                room.Width == int.Parse(width));
            }
            else
            {
                rooms.RemoveAll(room => room.RoomID == roomID);

            }
            seralizedRooms = JsonConvert.SerializeObject(rooms);
            HttpContext.Session.SetString("UpdateRooms", seralizedRooms);

            return View("UpdateHome");
        }

        [HttpPost]
        public IActionResult AddImage()
        {
            return View("UpdateHome");
        }

        [HttpPost]
        public IActionResult RemoveImage()
        {
            return View("UpdateHome");
        }
    }
}
