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
using System.IO;
using System.Net;

namespace Project4.Controllers
{
    //Handles Home Create, Modify and search
    public class RealEstateHomeController : Controller
    {
        private readonly IWebHostEnvironment webhostenvironment;

        public RealEstateHomeController(IWebHostEnvironment webHostEnvironment)
        {
            webhostenvironment = webHostEnvironment;
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
        //Button Controller for Home Form
        [HttpPost]
        public IActionResult HomeForm(string button)
        {
            //Gets the ID number of a button is it has an ID number. These are present in submit buttons dunamically generated
            int buttonNumber = button.Contains('_') ? int.Parse(button.Split('_').Last()) : -1;
            //handles which submit button was clicked
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
                    break;

            }
            return View("CreateHome");
        }
        //The following functions will add a div dynamically in the razor view 
        //or set a bool value that dictates if a div will be shown or hidden "deleted"
        public void AddRoom()
        {
            if (TempData["RoomCount"] == null)
            {
                TempData["RoomCount"] = 0;
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
                TempData["UtilityCount"] = 0;
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
                TempData["AmenityCount"] = 0;
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
                TempData["ImageCount"] = 0;
            }
            TempData["ImageCount"] = (int)TempData["ImageCount"] + 1;
            RetainData();
        }
        public void DeleteImage(int i)
        {
            TempData[$"ImageHidden_{i}"] = true;
            RetainData();
        }
        //Uploads image to the server
        public void UploadImage(int i)
        {
            string here = Request.Form[$"fuImage_{i}"];
            IFormFile file = Request.Form.Files[$"fuImage_{i}"];
            if (file == null || file.FileName.Split('.').Last() != "png")
            {
                //TODO: error
                return;
            }

            //TODO: Modify Image Learning Opportunity
            //-------------------------------------------------------
            ModifyImage modifyImage = new ModifyImage();

            //-------------------------------------------------------
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                modifyImage.Image = memoryStream.ToArray();
            }

            //Generate File Name
            string imageName = DateTime.Now.Ticks.ToString() + ".png";

            //get the server path 
            string serverPath = webhostenvironment.ContentRootPath;
            string path = Path.Combine(serverPath, "..", "Project3", "FileStorage");

            //Upload or display error when uploading image to server
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.CreateNew))
                {
                    fileStream.Write(modifyImage.Image, 0, modifyImage.Image.Length);
                }
                TempData[$"ImageURL_{i}"] = path;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"An error occurred while uploading the image. Path: {path} Image Name: {imageName} Error: {ex}";
                return;
            }
            //To remove the upload button from that specific image on the razor view
            TempData[$"ImageUploaded_{i}"] = true;
            RetainData();
        }
        //Code To Add Home to server through API
        public async Task AddHomeAsync(Home home)
        {
            StringContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(home), Encoding.UTF8, "application/json");
            string copy = System.Text.Json.JsonSerializer.Serialize(home);
            using (HttpClient httpClient = new HttpClient())
            {
                //TODO: show error if there is an error
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
        //Get Home Data
        public Home GetHomeData()
        {
            RetainData();
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
            for (int i = 0; i < int.Parse(TempData["ImageCount"].ToString()); i++)
            {
                var test = (RoomType)Enum.Parse(typeof(RoomType), Request.Form[$"ddlImageRoomType_{i}"]);
                var test2 = Request.Form[$"txtImageInformation_{i}"];
                images.Add(new Image(
                        //        (string)Request.Form[$"ImageURL{i}"],
                        "https://img.freepik.com/premium-vector/isolated-home-vector-illustration_1076263-25.jpg",
                        (RoomType)Enum.Parse(typeof(RoomType), Request.Form[$"ddlImageRoomType_{i}"].ToString()),
                        Request.Form[$"txtImageInformation_{i}"],
                        i == 1
                    ));
            }
            //read amenities
            Amenities amenities = new Amenities();
            for (int i = 0; i < int.Parse(TempData["AmenityCount"].ToString()); i++)
            {
                amenities.Add(new Amenity(
                        (AmenityType)Enum.Parse(typeof(AmenityType), Request.Form[$"ddlAmenityType_{i}"].ToString()),
                        Request.Form[$"txtAmenityInformation_{i}"]
                    ));
            }

            //read temperature control
            TemperatureControl temperatureControl = new TemperatureControl(
                    (HeatingTypes)Enum.Parse(typeof(HeatingTypes), Request.Form[$"ddlHeating"].ToString()),
                    (CoolingTypes)Enum.Parse(typeof(CoolingTypes), Request.Form[$"ddlCooling"].ToString())
                );
            //read rooms
            Rooms rooms = new Rooms();
            for (int i = 0; i < int.Parse(TempData["RoomCount"].ToString()); i++)
            {
                rooms.Add(new Room(
                        (RoomType)Enum.Parse(typeof(RoomType), Request.Form[$"ddlRoomType_{i}"].ToString()),
                        int.Parse(Request.Form[$"txtLength_{i}"]),
                        int.Parse(Request.Form[$"txtWidth_{i}"])
                    ));
            }

            //read Utilities
            Utilities utilities = new Utilities();
            for (int i = 0; i < int.Parse(TempData["UtilityCount"].ToString()); i++)
            {
                utilities.Add(new Project4.Models.Utility(
                        (UtilityTypes)Enum.Parse(typeof(UtilityTypes), Request.Form[$"ddlUtilityType_{i}"].ToString()),
                        Request.Form[$"txtUtilityInformation_{i}"]
                    ));
            }

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

        //=========================================================================================================

        public IActionResult AllEditHomes()
        {
            if (HttpContext.Session.GetString("Agent") == null) { return RedirectToAction("Dashboard", "Dashboard"); }
            string agentJson = HttpContext.Session.GetString("Agent");
            Agent currentAgent = JsonConvert.DeserializeObject<Agent>(agentJson);
            string apiUrl = "https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadHomeListings";
            WebRequest request = WebRequest.Create(apiUrl);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string data = reader.ReadToEnd();
            reader.Close();
            response.Close();
            Homes allHomes = JsonConvert.DeserializeObject<Homes>(data);
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

        public IActionResult EditHome(int? homeID)
        {
            if (HttpContext.Session.GetString("Agent") == null)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
            Home currentHome;
            if (homeID.HasValue)
            {
                string apiUrl = $"https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadSingleHomeListing/{homeID}";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                string jsonString = response.Content.ReadAsStringAsync().Result;
                currentHome = JsonConvert.DeserializeObject<Home>(jsonString);
                HttpContext.Session.SetString("EditHome", jsonString);
            }
            else
            {
                string json = HttpContext.Session.GetString("EditHome");
                currentHome = JsonConvert.DeserializeObject<Home>(json);
            }

            TempData["RoomCount"] = currentHome.Rooms?.List.Count;
            Console.WriteLine("Room Count: " + currentHome.Rooms.List.Count);
            for (int i = 0; i < currentHome.Rooms.List.Count; i++)
            {
                TempData[$"txtLength_{i}"] = currentHome.Rooms.List[i].Height;
                TempData[$"txtWidth_{i}"] = currentHome.Rooms.List[i].Width;
                TempData[$"ddlRoomType_{i}"] = currentHome.Rooms.List[i].Type;
                TempData[$"RoomHidden_{i}"] = false;
            }

            TempData["ImageCount"] = currentHome.Images.List.Count;
            Console.WriteLine("Image Count: " + currentHome.Images.List.Count);
            for (int i = 0; i < currentHome.Images.List.Count; i++)
            {
                TempData[$"ddlImageRoomType_{i}"] = currentHome.Images.List[i].Type;
                TempData[$"txtImageInformation_{i}"] = currentHome.Images.List[i].Description;
                TempData[$"ImageUploaded_{i}"] = true;
                TempData[$"ImageHidden_{i}"] = false;
            }

            TempData["AmenityCount"] = currentHome.Amenities.List.Count;
            Console.WriteLine("Amenity Count: " + currentHome.Amenities.List.Count);
            for (int i = 0; i < currentHome.Amenities.List.Count; i++)
            {
                TempData[$"txtAmenityInformation_{i}"] = currentHome.Amenities.List[i].Description;
                TempData[$"ddlAmenityType_{i}"] = currentHome.Amenities.List[i].Type;
                TempData[$"AmenityHidden_{i}"] = false;
            }

            TempData["UtilityCount"] = currentHome.Utilities.List.Count;
            Console.WriteLine("Utility Count: " + currentHome.Utilities.List.Count);
            for (int i = 0; i < currentHome.Utilities.List.Count; i++)
            {
                TempData[$"txtUtilityInformation_{i}"] = currentHome.Utilities.List[i].Information;
                TempData[$"ddlUtilityType_{i}"] = currentHome.Utilities.List[i].Type;
                TempData[$"UtilityHidden_{i}"] = false;
            }


            //HttpContext.Session.SetString("RoomCount", currentHome.Rooms.List.Count.ToString());
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


        [HttpPost]
        public IActionResult EditHomeForm(string button)
        {
            int buttonNumber = button.Contains('_') ? int.Parse(button.Split('_').Last()) : -1;
            switch (button.Split('_').First())
            {
                case "AddEditRoom":
                    AddRoom();
                    break;
                case "DeleteEditRoom":
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
                case "FinalizeEdit":
                    Home updatedHome = GetHomeData();
                    UpdateHome(updatedHome);
                    break;

            }
            return RedirectToAction("EditHome");
        }

        public void UpdateHome(Home updatedHome)
        {
            try
            {
                string apiUrl = "https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/UpdateHome/UpdateHomeListing";

                WebRequest request = WebRequest.Create(apiUrl);
                request.Method = "PUT";
                request.ContentType = "application/json";
                string jsonData = JsonConvert.SerializeObject(updatedHome);
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                using (WebResponse repsonse = request.GetResponse())
                {
                    using (Stream responseStream = repsonse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream);
                        string responseText = reader.ReadToEnd();
                        Console.WriteLine("Response: " + responseText);
                    }
                }
            }
            catch (WebException webEx)
            {
                // Handle HTTP-specific exceptions, such as bad requests or server errors
                if (webEx.Response != null)
                {
                    using (Stream responseStream = webEx.Response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            StreamReader reader = new StreamReader(responseStream);
                            string errorText = reader.ReadToEnd();
                            Console.WriteLine($"HTTP Error: {((HttpWebResponse)webEx.Response).StatusCode}");
                            Console.WriteLine("Error Details: " + errorText);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("WebException occurred: " + webEx.Message);
                }
            }
            catch (Exception ex)
            {
                // Handle other potential exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }




        }
        /*
		public async Task UpdateHome(Home updatedHome)
		{
			string jsonString = JsonConvert.SerializeObject(updatedHome);
			Console.WriteLine(jsonString);
			StringContent apiContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
			Console.WriteLine("We Made it to Update home");
			using (HttpClient httpClient = new HttpClient())
			{
				//TODO: show error if there is an error
				try
				{
					HttpResponseMessage response = await httpClient.PutAsync("https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/UpdateHome/UpdateHomeListing", apiContent);
					string responseBody = await response.Content.ReadAsStringAsync();
					Console.WriteLine($"Response Body: {responseBody}");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}

			Console.WriteLine("We Made it to past the api!");
		}
        */


        /*
         public Home GetHomeData()
        {
            RetainData();
            
            List<string> validationErrors = new List<string>();
            if (string.IsNullOrWhiteSpace(Request.Form["txtHomeStreet"]))
            {
                validationErrors.Add("Street address is required.");
                isValidHome = false;
            }
            if (string.IsNullOrWhiteSpace(Request.Form["txtHomeCity"]))
            {
                validationErrors.Add("City is required.");
                isValidHome = false;
            }
            if (!Enum.TryParse(typeof(States), Request.Form["ddlHomeState"], out _))
            {   
                validationErrors.Add("Invalid state selected.");
                isValidHome = false;
            }
            if (!int.TryParse(Request.Form["txtHomeCost"], out int cost))
            {
                validationErrors.Add("Cost must be a valid number.");
                isValidHome = false;
            }

            for (int i = 0; i < int.Parse(TempData["RoomCount"].ToString()); i++)
            {
                if (!Enum.TryParse(typeof(RoomType), Request.Form[$"ddlRoomType_{i}"], out _))
                {
                    validationErrors.Add($"Invalid room type for the {i + 1} room.");
                    isValidHome = false;
                }
                if (!int.TryParse(Request.Form[$"txtLength_{i}"], out _))
                {
                    validationErrors.Add($"Room {i + 1} length must be a valid number.");
                    isValidHome = false;
                }

                if (!int.TryParse(Request.Form[$"txtWidth_{i}"], out _))
                {
                    validationErrors.Add($"Room {i + 1} width must be a valid number.");
                    isValidHome = false;
                }
            }
            
            for (int i = 0; i < int.Parse(TempData["ImageCount"].ToString()); i++)
            {
                if (!Enum.TryParse(typeof(RoomType), Request.Form[$"ddlImageRoomType_{i}"], out _))
                {
                    validationErrors.Add($"Invalid room type for image {i + 1}.");
                    isValidHome = false;
                }
                if (string.IsNullOrWhiteSpace(Request.Form[$"txtImageInformation_{i}"]))
                {
                    validationErrors.Add($"Description is required for image {i + 1}.");
                    isValidHome = false;
                }
            }

            for (int i = 0; i < int.Parse(TempData["AmenityCount"].ToString()); i++)
            {
                if (!Enum.TryParse(typeof(AmenityType), Request.Form[$"ddlAmenityType_{i}"], out _))
                {
                    validationErrors.Add($"Invalid amenity type for amenity {i + 1}.");
                    isValidHome = false;
                }
                if (string.IsNullOrWhiteSpace(Request.Form[$"txtAmenityInformation_{i}"]))
                {
                    validationErrors.Add($"Description is required for amenity {i + 1}.");
                    isValidHome = false;
                }
            }

            for (int i = 0; i < int.Parse(TempData["UtilityCount"].ToString()); i++)
            {
                if (!Enum.TryParse(typeof(UtilityTypes), Request.Form[$"ddlUtilityType_{i}"], out _))
                {
                    validationErrors.Add($"Invalid utility type for utility {i + 1}.");
                    isValidHome = false;
                }
                if (string.IsNullOrWhiteSpace(Request.Form[$"txtUtilityInformation_{i}"]))
                {
                    validationErrors.Add($"Information is required for utility {i + 1}.");
                    isValidHome = false;
                }
            }

            
            if (isValidHome == false)
            {
                //Just need to add validation error span somewhere in create and edit home and test 
                //Also will need to make CreateHome and UpdateHome check for null home object
                TempData["ValidationError"] = validationErrors;
                return null;
            }
            else
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
            for(int i = 0; i < int.Parse(TempData["ImageCount"].ToString()); i++)
            {
                var test = (RoomType)Enum.Parse(typeof(RoomType), Request.Form[$"ddlImageRoomType_{i}"]);
                var test2 = Request.Form[$"txtImageInformation_{i}"];
                images.Add(new Image(
            //        (string)Request.Form[$"ImageURL{i}"],
                        "https://img.freepik.com/premium-vector/isolated-home-vector-illustration_1076263-25.jpg",
                        (RoomType)Enum.Parse(typeof(RoomType), Request.Form[$"ddlImageRoomType_{i}"].ToString()),
                        Request.Form[$"txtImageInformation_{i}"],
                        i == 1
                    ));
            }
            //read amenities
            Amenities amenities = new Amenities();
            for(int i = 0; i < int.Parse(TempData["AmenityCount"].ToString()); i++)
            {
                amenities.Add(new Amenity(
                        (AmenityType)Enum.Parse(typeof(AmenityType), Request.Form[$"ddlAmenityType_{i}"].ToString()),
                        Request.Form[$"txtAmenityInformation_{i}"]
                    ));
            }

            //read temperature control
            TemperatureControl temperatureControl = new TemperatureControl(
                    (HeatingTypes)Enum.Parse(typeof(HeatingTypes), Request.Form[$"ddlHeating"].ToString()),
                    (CoolingTypes)Enum.Parse(typeof(CoolingTypes), Request.Form[$"ddlCooling"].ToString())
                );
            //read rooms
            Rooms rooms = new Rooms();
            for(int i = 0; i < int.Parse(TempData["RoomCount"].ToString()); i++)
            {
                rooms.Add(new Room(
                        (RoomType)Enum.Parse(typeof(RoomType), Request.Form[$"ddlRoomType_{i}"].ToString()),
                        int.Parse(Request.Form[$"txtLength_{i}"]),
                        int.Parse(Request.Form[$"txtWidth_{i}"])
                    ));
            }

            //read Utilities
            Utilities utilities = new Utilities();
            for(int i = 0; i < int.Parse(TempData["UtilityCount"].ToString()); i++)
            {
                utilities.Add(new Project4.Models.Utility(
                        (UtilityTypes)Enum.Parse(typeof(UtilityTypes), Request.Form[$"ddlUtilityType_{i}"].ToString()),
                        Request.Form[$"txtUtilityInformation_{i}"]
                    ));
            }

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

        }
         */


    }
}
