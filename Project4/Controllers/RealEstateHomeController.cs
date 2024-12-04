using Azure.Identity;
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
            int buttonNumber = button.Contains('_')? int.Parse(button.Split('_').Last()) : -1;
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
                    Home home = GetHomeData();
                    AddHomeAsync(home);
                    RetainData();
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
                string fileStoragePath = Path.Combine(_environment.ContentRootPath,"TermProject", "FileStorage");
                if (!Directory.Exists(fileStoragePath))
                {
                    Directory.CreateDirectory(fileStoragePath);
                }
                using (FileStream fileStream = new FileStream(path, FileMode.CreateNew))
                {
                    fileStream.Write(modifyImage.Image, 0, modifyImage.Image.Length);
                }
                TempData[$"ImageURL{i}"] = path;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = "An error occurred while uploading the image." + path + " " + ex;
                return;
            }
            TempData[$"ImageUploaded_{i}"] = true;
            RetainData();
        }
        public async Task AddHomeAsync(Home home) 
        { 
            //TODO: Code To Add Home to server through API
            StringContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(home), Encoding.UTF8, "application/json");
            string copy = System.Text.Json.JsonSerializer.Serialize(home);
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync("https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/CreateHome/CreateHomeListing", content);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Body: {responseBody}");
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        //Get Home Data
        public Home GetHomeData()
        {
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
            Images images = new Images();
            //for(int i = 0; i < int.Parse(Request.Form["ImageCount"].ToString()); i++)
            //{
            //    images.Add(new Image(
            //            (string)Request.Form[$"ImageURL{i}"],
            //            (RoomType)Enum.Parse(typeof(RoomType), Request.Form[$"ddlImageRoomType{i}"].ToString()),
            //            (string)Request.Form[$"txtImageInformation{i}"],
            //           i == 1
            //        ));
            //}
            //read amenities
            Amenities amenities = new Amenities();
            //for(int i = 0; i < int.Parse(Request.Form["AmenityCount"].ToString()); i++)
            //{
                //amenities.Add(new Amenity(
                        //(AmenityType)Enum.Parse(typeof(AmenityType), Request.Form[$"ddlAmenityType{i}"].ToString()),
                        //(string)Request.Form[$"txtAmenityDescription{i}"]
                    //));
            //}

            //read temperature control
            TemperatureControl temperatureControl = new TemperatureControl(
                    (HeatingTypes)Enum.Parse(typeof(HeatingTypes), Request.Form[$"ddlHeating"].ToString()),
                    (CoolingTypes)Enum.Parse(typeof(CoolingTypes), Request.Form[$"ddlCooling"].ToString())
                );
            //read rooms
            Rooms rooms = new Rooms();
            for(int i = 0; i < int.Parse(Request.Form["RoomCount"].ToString()); i++)
            {
            int data = int.Parse(Request.Form[$"txtLength_{i}"]);
            var data2 = Request.Form[$"txtWidth_{i}"];
                rooms.Add(new Room(
                        (RoomType)Enum.Parse(typeof(RoomType), Request.Form[$"ddlRoomType{i}"].ToString()),
                        int.Parse(Request.Form[$"txtLength_{i}"]),
                        int.Parse(Request.Form[$"txtWidth_{i}"])
                    ));
            }

            //read Utilities
            Utilities utilities = new Utilities();
            //for(int i = 0; i < int.Parse(Request.Form["UtilityCount"].ToString()); i++)
            //{
                //utilities.Add(new Project4.Models.Utility(
                        //(UtilityTypes)Enum.Parse(typeof(UtilityTypes), Request.Form[$"ddlUtilityType_{i}"].ToString()),
                        //(string)Request.Form[$"txtUtilityDescription_{i}"]
                    //));
            //}


            //string testImage = "https://img.freepik.com/premium-vector/isolated-home-vector-illustration_1076263-25.jpg";

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
                images,
                amenities,
                temperatureControl,
                rooms,
                utilities
                );
            return home;
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
            ViewBag.Agent = currentAgent;
            return View();
        }
    }
}
