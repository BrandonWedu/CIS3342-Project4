﻿using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Project4.Models;
using System.Text;
using System.Text.Json;

namespace Project4.Controllers
{
    //Handles Home Create, Modify and search
    public class RealEstateHomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public RealEstateHomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        [HttpGet]
        public IActionResult CreateHome()
        {
            if (HttpContext.Session.GetString("Agent") == null)
            {
                return View("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult HomeForm(string button)
        {
            int buttonNumber = button.Contains('_') ? int.Parse(button.Split('_').Last()) : -1;
            switch (button.Split('_').First())
            {
                case "AddRoom":
                    AddRoom();
                    break;
                case "DeleteRoom":
                    DeleteRoom(buttonNumber);
                    break;
                case "AddUtility":
                    AddUtility();
                    break;
                case "DeleteUtility":
                    DeleteUtility(buttonNumber);
                    break;
                case "AddAmenity":
                    AddAmenity();
                    break;
                case "DeleteAmenity":
                    DeleteAmenity(buttonNumber);
                    break;
                case "AddImage":
                    AddImage();
                    break;
                case "DeleteImage":
                    DeleteImage(buttonNumber);
                    break;
                case "UploadImage":
                    UploadImage(buttonNumber);
                    break;
                case "AddHome":
                    AddHomeAsync();
                    break;

            }
            return View("CreateHome");
        }
        public void AddRoom()
        {
            if (TempData["RoomCount"] == null)
            {
                TempData["RoomCount"] = 1;
            }
            TempData["RoomCount"] = (int)TempData["RoomCount"] + 1;
            RetainData();
        }
        public void DeleteRoom(int i)
        {
            TempData[$"RoomHidden_{i}"] = true;
            RetainData();
        }
        public void AddUtility()
        {
            if (TempData["UtilityCount"] == null)
            {
                TempData["UtilityCount"] = 1;
            }
            TempData["UtilityCount"] = (int)TempData["UtilityCount"] + 1;
            RetainData();
        }
        public void DeleteUtility(int i)
        {
            TempData[$"UtilityHidden_{i}"] = true;
            RetainData();
        }
        public void AddAmenity()
        {
            if (TempData["AmenityCount"] == null)
            {
                TempData["AmenityCount"] = 1;
            }
            TempData["AmenityCount"] = (int)TempData["AmenityCount"] + 1;
            RetainData();
        }
        public void DeleteAmenity(int i)
        {
            TempData[$"AmenityHidden_{i}"] = true;
            RetainData();
        }
        public void AddImage()
        {
            if (TempData["ImageCount"] == null)
            {
                TempData["ImageCount"] = 1;
            }
            TempData["ImageCount"] = (int)TempData["ImageCount"] + 1;
            RetainData();
        }
        public void DeleteImage(int i)
        {
            TempData[$"ImageHidden_{i}"] = true;
            RetainData();
        }
        public void UploadImage(int i)
        {
            //Save Image To Server
            string here = Request.Form[$"fuImage_{i}"];
            IFormFile file = Request.Form.Files[$"fuImage_{i}"];
            if (file == null || file.FileName.Split('.').Last() != "png")
            {
                //error
                return;
            }

            //TODO: Modify Image Learning Opportunity
            //-------------------------------------------------------
            ModifyImage modifyImage = new ModifyImage();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                modifyImage.Image = memoryStream.ToArray();
            }


            //Generate File Name
            string imageName = DateTime.Now.Ticks.ToString() + ".png";
            //get the server path 
            string serverPath = _environment.WebRootPath;
            string path = Path.Combine(serverPath, "FileStorage", imageName);

            try
            {
                string fileStoragePath = Path.Combine(_environment.WebRootPath, "FileStorage");
                if (!Directory.Exists(fileStoragePath))
                {
                    Directory.CreateDirectory(fileStoragePath);
                }

                using (FileStream fileStream = new FileStream(path, FileMode.CreateNew))
                {
                    fileStream.Write(modifyImage.Image, 0, modifyImage.Image.Length);
                }
            }
            catch (Exception ex)
            {
                TempData["Errors"] = "An error occurred while uploading the image." + path + " " + ex;
                return;
            }
            TempData[$"ImageUploaded_{i}"] = true;
            RetainData();
        }
        public async Task AddHomeAsync()
        {
            //TODO: Code To Add Home to server through API
            string agentJson = HttpContext.Session.GetString("Agent");
            Agent agent = System.Text.Json.JsonSerializer.Deserialize<Agent>(agentJson);
            int cost = int.Parse(Request.Form["txtHomeCost"]);
            Address address = new Address(
                    Request.Form["txtHomeStreet"].ToString(),
                    Request.Form["txtHomeCity"].ToString(),
                    (States)Enum.Parse(typeof(States), Request.Form["ddlHomeState"].ToString()),
                    Request.Form["txtHomeZipCode"].ToString()
                );
            PropertyType propertyType = (PropertyType)Enum.Parse(typeof(PropertyType), Request.Form["ddlPropertyType"].ToString());
            GarageType garageType = (GarageType)Enum.Parse(typeof(GarageType), Request.Form["ddlGarageType"].ToString());
            string description = Request.Form["txtDescription"].ToString();
            SaleStatus saleStatus = (SaleStatus)Enum.Parse(typeof(SaleStatus), Request.Form["ddlSaleStatus"].ToString());
            //read images

            //read amenities

            //read temperature control

            //read rooms

            //read Utilities

            string testImage = "https://img.freepik.com/premium-vector/isolated-home-vector-illustration_1076263-25.jpg";

            Home home = new Home(
                agent.AgentID,
                cost,
                address,
                propertyType,
                DateTime.Now.Year,
                garageType,
                description,
                DateTime.Now,
                saleStatus,
                new Images(),
                new Amenities(),
                new TemperatureControl(HeatingTypes.CentralHeating, CoolingTypes.CentralAir),
                new Rooms(),
                new Utilities()
                );
            //Call the Email API and send the email
            StringContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(home), Encoding.UTF8, "application/json");
            string copy = System.Text.Json.JsonSerializer.Serialize(home);
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync("https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/CreateHome/CreateHomeListing", content);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Body: {responseBody}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        //save request.form to temp data
        public void RetainData()
        {
            foreach (string key in Request.Form.Keys)
            {
                TempData[key] = Request.Form[key];
            }
            TempData.Keep();
        }



        public IActionResult AllEditHomes()
        {
            string agentJson = HttpContext.Session.GetString("Agent");
            Agent currentAgent = JsonConvert.DeserializeObject<Agent>(agentJson);
            string apiUrl = "https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadHomeListings";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            string jsonString = response.Content.ReadAsStringAsync().Result;
            Homes allHomes = JsonConvert.DeserializeObject<Homes>(jsonString);
            Homes agentHomes = new Homes();
            foreach (Home currentHome in allHomes.List)
            {
                Console.WriteLine("Home ID: " + currentHome.HomeID + " , AgentID: " + currentHome.AgentID + " , StoredAgentID: " + currentAgent.AgentID);
                if (currentHome.AgentID.ToString() == currentAgent.AgentID.ToString())
                {
                    agentHomes.List.Add(currentHome);
                    Console.WriteLine("Added Home To List");
                }
            }
            Console.WriteLine("List Count: " + agentHomes.List.Count);

            ViewBag.Agent = currentAgent;
            ViewBag.AgentHomes = agentHomes;
            return View();
        }


        public IActionResult EditHome(int homeID)
        {

            if (HttpContext.Session.GetString("EditHome") == null)
            {
                string agentJson = HttpContext.Session.GetString("Agent");
                Agent currentAgent = JsonConvert.DeserializeObject<Agent>(agentJson);
                string apiUrl = $"https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadSingleHomeListing/{homeID}";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                string jsonString = response.Content.ReadAsStringAsync().Result;
                Home currentHome = JsonConvert.DeserializeObject<Home>(jsonString);

                Rooms homeRooms = currentHome.Rooms;
                Images homeImages = currentHome.Images;
                Amenities homeAmenities = currentHome.Amenities;
                Utilities homeUtilities = currentHome.Utilities;
                TemperatureControl homeTemperature = currentHome.TemperatureControl;
                string roomsJson = JsonConvert.SerializeObject(homeRooms);
                string imagesJson = JsonConvert.SerializeObject(homeImages);
                string amenitiesJson = JsonConvert.SerializeObject(homeAmenities);
                string utilitiesJson = JsonConvert.SerializeObject(homeUtilities);
                string temperatureJson = JsonConvert.SerializeObject(homeTemperature);
                HttpContext.Session.SetString("EditRooms", roomsJson);
                HttpContext.Session.SetString("EditImages", imagesJson);
                HttpContext.Session.SetString("EditAmenities", amenitiesJson);
                HttpContext.Session.SetString("EditUtilities", utilitiesJson);
                HttpContext.Session.SetString("EditHome", jsonString);
                HttpContext.Session.SetString("EditTemperature", temperatureJson);
                ViewBag.EditRooms = homeRooms;
                ViewBag.EditImages = homeImages;
                ViewBag.EditAmenities = homeAmenities;
                ViewBag.EditUtilities = homeUtilities;
                ViewBag.EditTemperature = homeTemperature;
                TempData["txtHomeStreet"] = currentHome.Address.Street;
                TempData["txtHomeCity"] = currentHome.Address.City;
                TempData["ddlHomeState"] = currentHome.Address.State;
                TempData["txtHomeZipCode"] = currentHome.Address.ZipCode;
                TempData["txtHomeCost"] = currentHome.Cost;
                TempData["ddlPropertyType"] = currentHome.PropertyType;
                TempData["txtYearConstructed"] = currentHome.YearConstructed;
                TempData["ddlGarageType"] = currentHome.GarageType;
                TempData["txtHomeDescription"] = currentHome.Description;
                TempData["ddlSaleStatus"] = currentHome.SaleStatus;
                TempData["ddlCooling"] = currentHome.TemperatureControl.Cooling;
                TempData["ddlHeating"] = currentHome.TemperatureControl.Heating;


                return View("EditHome");
            }
            else
            {
                string roomsJson = HttpContext.Session.GetString("EditRooms");
                string imagesJson = HttpContext.Session.GetString("EditImages");
                string amenitiesJson = HttpContext.Session.GetString("EditAmenities");
                string utilitiesJson = HttpContext.Session.GetString("EditUtilities");
                string temperatureJson = HttpContext.Session.GetString("EditTemperature");
                string homeJson = HttpContext.Session.GetString("EditHome");
                Rooms homeRooms = JsonConvert.DeserializeObject<Rooms>(roomsJson);
                Images homeImages = JsonConvert.DeserializeObject<Images>(imagesJson);
                Amenities homeAmenities = JsonConvert.DeserializeObject<Amenities>(amenitiesJson);
                Utilities homeUtilities = JsonConvert.DeserializeObject<Utilities>(utilitiesJson);
                TemperatureControl homeTemperature = JsonConvert.DeserializeObject<TemperatureControl>(temperatureJson);
                Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
                ViewBag.EditRooms = homeRooms;
                ViewBag.EditImages = homeImages;
                ViewBag.EditAmenities = homeAmenities;
                ViewBag.EditUtilities = homeUtilities;
                ViewBag.EditTemperature = homeTemperature;
                TempData["txtHomeStreet"] = currentHome.Address.Street;
                TempData["txtHomeCity"] = currentHome.Address.City;
                TempData["ddlHomeState"] = currentHome.Address.State;
                TempData["txtHomeZipCode"] = currentHome.Address.ZipCode;
                TempData["txtHomeCost"] = currentHome.Cost;
                TempData["ddlPropertyType"] = currentHome.PropertyType;
                TempData["txtYearConstructed"] = currentHome.YearConstructed;
                TempData["ddlGarageType"] = currentHome.GarageType;
                TempData["txtHomeDescription"] = currentHome.Description;
                TempData["ddlSaleStatus"] = currentHome.SaleStatus;
                TempData["ddlCooling"] = currentHome.TemperatureControl.Cooling;
                TempData["ddlHeating"] = currentHome.TemperatureControl.Heating;
                return View("EditHome");

            }

        }

        public IActionResult AddEditRoom()
        {
            string roomsJson = HttpContext.Session.GetString("EditRooms");
            string imagesJson = HttpContext.Session.GetString("EditImages");
            string amenitiesJson = HttpContext.Session.GetString("EditAmenities");
            string utilitiesJson = HttpContext.Session.GetString("EditUtilities");
            string temperatureJson = HttpContext.Session.GetString("EditTemperature");
            string homeJson = HttpContext.Session.GetString("EditHome");
            Rooms homeRooms = JsonConvert.DeserializeObject<Rooms>(roomsJson);
            Images homeImages = JsonConvert.DeserializeObject<Images>(imagesJson);
            Amenities homeAmenities = JsonConvert.DeserializeObject<Amenities>(amenitiesJson);
            Utilities homeUtilities = JsonConvert.DeserializeObject<Utilities>(utilitiesJson);
            TemperatureControl homeTemperature = JsonConvert.DeserializeObject<TemperatureControl>(temperatureJson);
            Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
            homeRooms.List.Add(new Room());
            currentHome.Rooms = homeRooms;
            string reseralizedRoomJson = JsonConvert.SerializeObject(homeRooms);
            string researlizedHomeJson = JsonConvert.SerializeObject(currentHome);
            HttpContext.Session.SetString("EditRooms", reseralizedRoomJson);
            HttpContext.Session.SetString("EditHome", researlizedHomeJson);



            return RedirectToAction("EditHome", currentHome.HomeID);
        }
        [HttpPost]
        [Route("RealEstateHome/RemoveEditRoom/{roomCount}")]
        public IActionResult RemoveEditRoom(int roomCount)
        {
            string roomsJson = HttpContext.Session.GetString("EditRooms");
            string imagesJson = HttpContext.Session.GetString("EditImages");
            string amenitiesJson = HttpContext.Session.GetString("EditAmenities");
            string utilitiesJson = HttpContext.Session.GetString("EditUtilities");
            string temperatureJson = HttpContext.Session.GetString("EditTemperature");
            string homeJson = HttpContext.Session.GetString("EditHome");
            Rooms homeRooms = JsonConvert.DeserializeObject<Rooms>(roomsJson);
            Images homeImages = JsonConvert.DeserializeObject<Images>(imagesJson);
            Amenities homeAmenities = JsonConvert.DeserializeObject<Amenities>(amenitiesJson);
            Utilities homeUtilities = JsonConvert.DeserializeObject<Utilities>(utilitiesJson);
            TemperatureControl homeTemperature = JsonConvert.DeserializeObject<TemperatureControl>(temperatureJson);
            Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
            homeRooms.List.RemoveAt(roomCount);
            currentHome.Rooms = homeRooms;
            string reseralizedRoomJson = JsonConvert.SerializeObject(homeRooms);
            string researlizedHomeJson = JsonConvert.SerializeObject(currentHome);
            HttpContext.Session.SetString("EditRooms", reseralizedRoomJson);
            HttpContext.Session.SetString("EditHome", researlizedHomeJson);

            return RedirectToAction("EditHome", currentHome.HomeID);
        }

        public IActionResult TryFinalizeEditHome()
        {
            Console.WriteLine("Made It To TryFinalize!");
            //Add validation of each object from session to make sure each object is completely filled out
            string roomsJson = HttpContext.Session.GetString("EditRooms");
            string imagesJson = HttpContext.Session.GetString("EditImages");
            string amenitiesJson = HttpContext.Session.GetString("EditAmenities");
            string utilitiesJson = HttpContext.Session.GetString("EditUtilities");
            string temperatureJson = HttpContext.Session.GetString("EditTemperature");
            string homeJson = HttpContext.Session.GetString("EditHome");
            Rooms homeRooms = JsonConvert.DeserializeObject<Rooms>(roomsJson);
            Images homeImages = JsonConvert.DeserializeObject<Images>(imagesJson);
            Amenities homeAmenities = JsonConvert.DeserializeObject<Amenities>(amenitiesJson);
            Utilities homeUtilities = JsonConvert.DeserializeObject<Utilities>(utilitiesJson);
            TemperatureControl homeTemperature = JsonConvert.DeserializeObject<TemperatureControl>(temperatureJson);
            Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
            //Validate each smaller componet and after validation readd them to the home object

            //Do api call
            HttpClient client = new HttpClient();
            StringContent apiContent = new StringContent(JsonConvert.SerializeObject(currentHome), Encoding.UTF8, "application/json");
            HttpResponseMessage apiResponse = client.PostAsync("https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/UpdateHome/UpdateHomeListing", apiContent).Result;
            Console.WriteLine(apiContent.ToString());
            Console.WriteLine(apiResponse);

            HttpContext.Session.Remove("EditRooms");
            HttpContext.Session.Remove("EditImages");
            HttpContext.Session.Remove("EditAmenities");
            HttpContext.Session.Remove("EditUtilities");
            HttpContext.Session.Remove("EditTemperature");
            HttpContext.Session.Remove("EditHome");
            return RedirectToAction("EditHome", currentHome.HomeID);

        }
    }
}
