using Microsoft.AspNetCore.Mvc;
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
            if (HttpContext.Session.GetString("Agent") == null) { return RedirectToAction("Dashboard", "Dashboard"); }
            return View();
        }
        //Button Controller for Home Form
        [HttpPost]
        public IActionResult HomeForm(string button)
        {
            if (HttpContext.Session.GetString("Agent") == null) { return RedirectToAction("Dashboard", "Dashboard"); }
            //Gets the ID number of a button is it has an ID number. These are present in submit buttons dynamically generated
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
                    AddHome(home);
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
		public void AddHome(Home home)
		{
			if (home == null)
			{
				Console.WriteLine("Home was null, failed validation");
				TempData["Response"] = "Invalid home data";
				return;
			}

			StringContent content = new StringContent(
				System.Text.Json.JsonSerializer.Serialize(home),
				Encoding.UTF8,
				"application/json"
			);

			using (HttpClient httpClient = new HttpClient())
			{
				try
				{
					// Blocking call to PostAsync
					HttpResponseMessage response = httpClient.PostAsync(
						"https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/CreateHome/CreateHomeListing",
						content
					).GetAwaiter().GetResult();

					// Blocking call to ReadAsStringAsync
					string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
					Console.WriteLine($"Response Body: {responseBody}");

					// Error handling
					if (responseBody == "-1")
					{
						// Home already exists
						TempData["Response"] = "Error, a home with this address already exists";
					}
					else
					{
						TempData["Response"] = "Home Created";
					}
				}
				catch (Exception ex)
				{
					// Handle exceptions
					Console.WriteLine($"Exception: {ex.Message}");
					TempData["Response"] = "Web API Connection Error";
				}
			}
		}
		//Get Home Data
		public Home GetHomeData()
        {
            RetainData();

            List<string> validationErrors = new List<string>();
            bool isValidHome = true;
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
			if (string.IsNullOrWhiteSpace(Request.Form["txtHomeZipCode"]))
			{
				validationErrors.Add("Zip Code is required.");
				isValidHome = false;
			}
            if (!int.TryParse(Request.Form["txtHomeCost"], out int cost))
            {
                validationErrors.Add("Cost must be a valid number.");
                isValidHome = false;
            }
			if (!int.TryParse(Request.Form["txtYearConstructed"], out int yearBuilt))
			{
				validationErrors.Add("Year built must be a valid number.");
				isValidHome = false;
			}
			if (string.IsNullOrWhiteSpace(Request.Form["txtHomeDescription"]))
			{
				validationErrors.Add("Home description is required.");
				isValidHome = false;
			}


			//room
            if (TempData["RoomCount"] != null)
            {
                int roomCount = int.Parse(TempData["RoomCount"].ToString());
                int nonHiddenRoomCount = 0;
                for (int i = 0; i < roomCount; i++)
                {
                    if (!(TempData[$"RoomHidden_{i}"] is bool hidden && hidden))
                    {
                        nonHiddenRoomCount++;
						string roomType = Request.Form[$"ddlRoomType_{i}"];
						string length = Request.Form[$"txtLength_{i}"];
						string width = Request.Form[$"txtWidth_{i}"];
						if (string.IsNullOrWhiteSpace(roomType) || !Enum.TryParse(typeof(RoomType), roomType, out _))
                        {
							validationErrors.Add($"Valid room type is required for room {i + 1}.");
						}	
						if (!int.TryParse(length, out _) || !int.TryParse(width, out _))
                        {
							validationErrors.Add($"Length and width must be valid numbers for room {i + 1}.");
						}

					}
                }
                if (nonHiddenRoomCount == 0)
                {
                    isValidHome = false;
                    validationErrors.Add("At least one room is required");
                }
            }
            else
            {
                isValidHome = false;
                validationErrors.Add("At least one room is required.");
            }

			// Images
			if (TempData["ImageCount"] != null)
			{
				int imageCount = int.Parse(TempData["ImageCount"].ToString());
				int nonHiddenImageCount = 0;
				for (int i = 0; i < imageCount; i++)
				{
					if (!(TempData[$"ImageHidden_{i}"] is bool hidden && hidden))
					{
						nonHiddenImageCount++;
						string roomType = Request.Form[$"ddlImageRoomType_{i}"];
						string imageInfo = Request.Form[$"txtImageInformation_{i}"];

						if (string.IsNullOrWhiteSpace(roomType) || !Enum.TryParse(typeof(RoomType), roomType, out _))
						{
							validationErrors.Add($"Valid room type is required for image {i + 1}.");
						}
						if (string.IsNullOrWhiteSpace(imageInfo))
						{
							validationErrors.Add($"Image information is required for image {i + 1}.");
						}
					}
				}
				if (nonHiddenImageCount == 0)
				{
					isValidHome = false;
					validationErrors.Add("At least one image is required.");
				}
			}
			else
			{
				isValidHome = false;
				validationErrors.Add("At least one image is required.");
			}




			// Amenities
			if (TempData["AmenityCount"] != null)
			{
				int amenityCount = int.Parse(TempData["AmenityCount"].ToString());
				int nonHiddenAmenityCount = 0;
				for (int i = 0; i < amenityCount; i++)
				{
					if (!(TempData[$"AmenityHidden_{i}"] is bool hidden && hidden))
					{
						nonHiddenAmenityCount++;
						string amenityType = Request.Form[$"ddlAmenityType_{i}"];
						string amenityInfo = Request.Form[$"txtAmenityInformation_{i}"];

						if (string.IsNullOrWhiteSpace(amenityType) || !Enum.TryParse(typeof(AmenityType), amenityType, out _))
						{
							validationErrors.Add($"Valid amenity type is required for amenity {i + 1}.");
						}
						if (string.IsNullOrWhiteSpace(amenityInfo))
						{
							validationErrors.Add($"Amenity information is required for amenity {i + 1}.");
						}
					}
				}
				if (nonHiddenAmenityCount == 0)
				{
					isValidHome = false;
					validationErrors.Add("At least one amenity is required.");
				}
			}
			else
			{
				isValidHome = false;
				validationErrors.Add("At least one amenity is required.");
			}




			// Utility Validation
			if (TempData["UtilityCount"] != null)
			{
				int utilityCount = int.Parse(TempData["UtilityCount"].ToString());
				int nonHiddenUtilityCount = 0;
				for (int i = 0; i < utilityCount; i++)
				{
					if (!(TempData[$"UtilityHidden_{i}"] is bool hidden && hidden))
					{
						nonHiddenUtilityCount++;
						string utilityType = Request.Form[$"ddlUtilityType_{i}"];
						string utilityInfo = Request.Form[$"txtUtilityInformation_{i}"];

						if (string.IsNullOrWhiteSpace(utilityType) || !Enum.TryParse(typeof(UtilityTypes), utilityType, out _))
						{
							validationErrors.Add($"Valid utility type is required for utility {i + 1}.");
						}
						if (string.IsNullOrWhiteSpace(utilityInfo))
						{
							validationErrors.Add($"Utility information is required for utility {i + 1}.");
						}
					}
				}
				if (nonHiddenUtilityCount == 0)
				{
					isValidHome = false;
					validationErrors.Add("At least one utility is required.");
				}
			}
			else
			{
				isValidHome = false;
				validationErrors.Add("At least one utility is required.");
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
                cost = int.Parse(Request.Form["txtHomeCost"]);
                Address address = new Address(
                        Request.Form["txtHomeStreet"].ToString(),
                        Request.Form["txtHomeCity"].ToString(),
                        (States)Enum.Parse(typeof(States), Request.Form["ddlHomeState"].ToString()),
                        Request.Form["txtHomeZipCode"].ToString()
                    );
                PropertyType propertyType = (PropertyType)Enum.Parse(typeof(PropertyType), Request.Form["ddlPropertyType"].ToString());
                GarageType garageType = (GarageType)Enum.Parse(typeof(GarageType), Request.Form["ddlGarageType"].ToString());
                string description = Request.Form["txtHomeDescription"].ToString();
                SaleStatus saleStatus = (SaleStatus)Enum.Parse(typeof(SaleStatus), Request.Form["ddlSaleStatus"].ToString());

                //read images
                Images images = new Images();
                for (int i = 0; i < int.Parse(TempData["ImageCount"].ToString()); i++)
                {
                    if (TempData[$"ImageHidden_{i}"] == null ||(bool)TempData[$"ImageHidden_{i}"] == false)
                    {
						//var test = (RoomType)Enum.Parse(typeof(RoomType), Request.Form[$"ddlImageRoomType_{i}"]);
						//var test2 = Request.Form[$"txtImageInformation_{i}"];
						images.Add(new Image(
								//        (string)Request.Form[$"ImageURL{i}"],
								"https://img.freepik.com/premium-vector/isolated-home-vector-illustration_1076263-25.jpg",
								(RoomType)Enum.Parse(typeof(RoomType), Request.Form[$"ddlImageRoomType_{i}"].ToString()),
								Request.Form[$"txtImageInformation_{i}"],
								i == 1
							));
                    }
                }
                //read amenities
                Amenities amenities = new Amenities();
                for (int i = 0; i < int.Parse(TempData["AmenityCount"].ToString()); i++)
                {
                    if (TempData[$"AmenityHidden_{i}"] == null || (bool)TempData[$"AmenityHidden_{i}"] == false)
                    {
                        amenities.Add(new Amenity(
                                (AmenityType)Enum.Parse(typeof(AmenityType), Request.Form[$"ddlAmenityType_{i}"].ToString()),
                                Request.Form[$"txtAmenityInformation_{i}"]
						));
                    }
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
                    if (TempData[$"RoomHidden_{i}"] == null || (bool)TempData[$"RoomHidden_{i}"] == false)
                    {
                        rooms.Add(new Room(
                                (RoomType)Enum.Parse(typeof(RoomType), Request.Form[$"ddlRoomType_{i}"].ToString()),
                                int.Parse(Request.Form[$"txtLength_{i}"]),
                                int.Parse(Request.Form[$"txtWidth_{i}"])
                            ));
                    }
                }

                //read Utilities
                Utilities utilities = new Utilities();
                for (int i = 0; i < int.Parse(TempData["UtilityCount"].ToString()); i++)
                {
                    if (TempData[$"RoomHidden_{i}"] == null || (bool)TempData[$"RoomHidden_{i}"] == false)
                    {
                        utilities.Add(new Project4.Models.Utility(
                                (UtilityTypes)Enum.Parse(typeof(UtilityTypes), Request.Form[$"ddlUtilityType_{i}"].ToString()),
                                Request.Form[$"txtUtilityInformation_{i}"]
                            ));
                    }
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
                    agentHomes.Add(currentHome);
                    Console.WriteLine("Added Home To List");
                }
            }
            Console.WriteLine("List Count: " + agentHomes.List.Count);
            ViewBag.Agent = currentAgent;
            ViewBag.AgentHomes = agentHomes;
            return View();
        }

        public async Task UpdateHome(Home updatedHome)
        {
            try
            {
                string apiUrl = "https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/UpdateHome/UpdateHomeListing";

                using (HttpClient client = new HttpClient())
                {
                    string jsonData = JsonConvert.SerializeObject(updatedHome);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(apiUrl, content);


                    Console.WriteLine(jsonData);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response:" + responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"HTTP Error:  {response.StatusCode}");
                        string errorBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Error Details:" + errorBody);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle other potential exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }




        }

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

        }
         */


		public IActionResult BackupEditHome(int homeID)
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

				return View("BackupEditHome");
			
			

		}

        public IActionResult BackupEditHomeLoad()
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
			return View("BackupEditHome");
        }

		public IActionResult AddEditRoom(string roomLength, string roomWidth, string roomType)
		{
			string roomsJson = HttpContext.Session.GetString("EditRooms");
			string homeJson = HttpContext.Session.GetString("EditHome");
			Rooms homeRooms = JsonConvert.DeserializeObject<Rooms>(roomsJson);
			Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
			homeRooms.Add(new Room(0, Enum.Parse<RoomType>(roomType), int.Parse(roomLength), int.Parse(roomWidth)));
			currentHome.Rooms = homeRooms;
			string reseralizedRoomJson = JsonConvert.SerializeObject(homeRooms);
			string researlizedHomeJson = JsonConvert.SerializeObject(currentHome);
			HttpContext.Session.SetString("EditRooms", reseralizedRoomJson);
			HttpContext.Session.SetString("EditHome", researlizedHomeJson);
			return RedirectToAction("BackupEditHomeLoad");
		}

		[HttpPost]
		[Route("RealEstateHome/RemoveEditRoom/{roomCount}")]
		public IActionResult RemoveEditRoom(int roomCount)
		{
			string roomsJson = HttpContext.Session.GetString("EditRooms");
			string homeJson = HttpContext.Session.GetString("EditHome");
			Rooms homeRooms = JsonConvert.DeserializeObject<Rooms>(roomsJson);
			Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
			homeRooms.RemoveAtIndex(roomCount);
			currentHome.Rooms = homeRooms;
			string reseralizedRoomJson = JsonConvert.SerializeObject(homeRooms);
			string researlizedHomeJson = JsonConvert.SerializeObject(currentHome);
			HttpContext.Session.SetString("EditRooms", reseralizedRoomJson);
			HttpContext.Session.SetString("EditHome", researlizedHomeJson);
			return RedirectToAction("BackupEditHomeLoad");
		}

		public IActionResult AddEditUtility(string utilityType, string utilityInformation)
		{

			string utilitiesJson = HttpContext.Session.GetString("EditUtilities");
			string homeJson = HttpContext.Session.GetString("EditHome");
			Utilities homeUtilities = JsonConvert.DeserializeObject<Utilities>(utilitiesJson);
			Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
			homeUtilities.Add(new Utility(0, Enum.Parse<UtilityTypes>(utilityType), utilityInformation));
			currentHome.Utilities = homeUtilities;
			string researlizedUtilities = JsonConvert.SerializeObject(homeUtilities);
			string researlizedHomeJson = JsonConvert.SerializeObject(currentHome);
			HttpContext.Session.SetString("EditUtilities", researlizedUtilities);
			HttpContext.Session.SetString("EditHome", researlizedHomeJson);
			return RedirectToAction("BackupEditHomeLoad");
		}

		public IActionResult AddEditAmenity(string amenityType, string amenityInformation)
		{
			string amenitiesJson = HttpContext.Session.GetString("EditAmenities");
			string homeJson = HttpContext.Session.GetString("EditHome");
			Amenities homeAmenities = JsonConvert.DeserializeObject<Amenities>(amenitiesJson);
			Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
			homeAmenities.Add(new Amenity(0, Enum.Parse<AmenityType>(amenityType), amenityInformation.ToString()));
			currentHome.Amenities = homeAmenities;
			string researlizedAmenityJson = JsonConvert.SerializeObject(homeAmenities);
			string researlizedHomeJson = JsonConvert.SerializeObject(currentHome);
			HttpContext.Session.SetString("EditAmenities", researlizedAmenityJson);
			HttpContext.Session.SetString("EditHome", researlizedHomeJson);
			return RedirectToAction("BackupEditHomeLoad");
		}

        public IActionResult AddEditImage(string imageDescription, string imageType, IFormFile imageFile, string mainImage, string defaultImage)
        {
            if (imageFile == null || imageFile.FileName.Split('.').Last() != "png")
            {
				Console.WriteLine("Image Error");
				//error
				ViewBag.EditHomeError = "IncorrectImage Type/ could not find image";
				return View("BackupEditHomeLoad");

			}
            else
            {
				string id = DateTime.Now.Ticks.ToString();
				string imageName = id + ".png";
				string relativePath = $"FileStorage/{id}"; // Relative path to the file
				string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath); // Absolute path to the file

				if (!Directory.Exists(relativePath))
				{
					Directory.CreateDirectory(absolutePath);
				}

				if (imageFile.Length > 0)
				{
					string fileExtension = ".png";
					string newFileName = $"{id}{fileExtension}";
					string fullPath = Path.Combine(absolutePath, newFileName);
					Console.WriteLine(fullPath);

					using (var fileStream = new FileStream(fullPath, FileMode.Create))
					{
						imageFile.CopyTo(fileStream);
					}

					string imagesJson = HttpContext.Session.GetString("EditImages");
					string homeJson = HttpContext.Session.GetString("EditHome");
					Images homeImagees = JsonConvert.DeserializeObject<Images>(imagesJson);
					Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
					homeImagees.Add(new Image(0, fullPath, Enum.Parse<RoomType>(imageType), imageDescription, false));
					currentHome.Images = homeImagees;
					string researlizedImagesJson = JsonConvert.SerializeObject(homeImagees);
					string researlizedHomeJson = JsonConvert.SerializeObject(currentHome);
					HttpContext.Session.SetString("EditImages", researlizedImagesJson);
					HttpContext.Session.SetString("EditHome", researlizedHomeJson);
					return RedirectToAction("BackupEditHomeLoad");
				}
                else
                {
                    ViewBag.EditHomeError = "Image File Empty";
					return RedirectToAction("BackupEditHomeLoad");
				}
			}
		}

		[HttpPost]
		[Route("RealEstateHome/RemoveEditImage/{imageCount}")]
		public IActionResult RemoveEditImage(int imageCount)
		{

			string imagesJson = HttpContext.Session.GetString("EditImages");
			string homeJson = HttpContext.Session.GetString("EditHome");
			Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
            Images homeImages = JsonConvert.DeserializeObject<Images>(imagesJson);
			Image removedImage = homeImages.List[imageCount];
			homeImages.RemoveAtIndex(imageCount);


				string imagePath = removedImage.Url.ToString();
				if (System.IO.File.Exists(imagePath))
				{
					System.IO.File.Delete(imagePath);
				}


			currentHome.Images = homeImages;
			string researlizedImagesJson = JsonConvert.SerializeObject(homeImages);
			string researlizedHomeJson = JsonConvert.SerializeObject(currentHome);
			HttpContext.Session.SetString("EditImages", researlizedImagesJson);
			HttpContext.Session.SetString("EditHome", researlizedHomeJson);

			return RedirectToAction("BackupEditHomeLoad");
		}

		[HttpPost]
		[Route("RealEstateHome/RemoveEditAmenity/{amenityCount}")]
		public IActionResult RemoveEditAmenity(int amenityCount)
		{

			string amenitiesJson = HttpContext.Session.GetString("EditAmenities");
			string homeJson = HttpContext.Session.GetString("EditHome");
			Amenities homeAmenities = JsonConvert.DeserializeObject<Amenities>(amenitiesJson);
			Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
            homeAmenities.RemoveAtIndex(amenityCount);
            string reserlizedAmenitiesJson = JsonConvert.SerializeObject(homeAmenities);
            string reseralizedHomeJson = JsonConvert.SerializeObject(currentHome);
		    HttpContext.Session.SetString("EditAmenities", reserlizedAmenitiesJson);
			HttpContext.Session.SetString("EditHome", reseralizedHomeJson);
			return RedirectToAction("BackupEditHomeLoad", currentHome.HomeID);
		}

		[HttpPost]
		[Route("RealEstatehome/RemoveEditUtility/{utilityID}")]
		public IActionResult RemoveEditUtility(int utilityCount)
		{

			string utilitiesJson = HttpContext.Session.GetString("EditUtilities");
			string homeJson = HttpContext.Session.GetString("EditHome");
			Utilities homeUtilities = JsonConvert.DeserializeObject<Utilities>(utilitiesJson);
			Home currentHome = JsonConvert.DeserializeObject<Home>(homeJson);
			homeUtilities.RemoveAtIndex(utilityCount);
			currentHome.Utilities = homeUtilities;
			string researlizedUtilities = JsonConvert.SerializeObject(homeUtilities);
			string researlizedHomeJson = JsonConvert.SerializeObject(currentHome);
			HttpContext.Session.SetString("EditUtilities", researlizedUtilities);
			HttpContext.Session.SetString("EditHome", researlizedHomeJson);
			return RedirectToAction("BackupEditHomeLoad", currentHome.HomeID);
		}

        public IActionResult TryFinalizeEditHome()
        {
            string updatedHomeJson = HttpContext.Session.GetString("EditHome");
            Home updatedHome = JsonConvert.DeserializeObject<Home>(updatedHomeJson);

			updatedHome.Address.Street = Request.Form["txtHomeStreet"];
			updatedHome.Address.City = Request.Form["txtHomeCity"];
			updatedHome.Address.State = Enum.Parse<States>(Request.Form["ddlHomeState"]);
			updatedHome.Address.ZipCode = Request.Form["txtHomeZipCode"];
			updatedHome.Cost = int.Parse(Request.Form["txtHomeCost"]);
			updatedHome.PropertyType = Enum.Parse<PropertyType>(Request.Form["ddlPropertyType"]);
			updatedHome.YearConstructed = int.Parse(Request.Form["txtYearConstructed"]);
			updatedHome.GarageType = Enum.Parse<GarageType>(Request.Form["ddlGarageType"]);
			updatedHome.Description = Request.Form["txtHomeDescription"];
			updatedHome.SaleStatus = Enum.Parse<SaleStatus>(Request.Form["ddlSaleStatus"]);
			updatedHome.TemperatureControl.Cooling = Enum.Parse<CoolingTypes>(Request.Form["ddlCooling"]);
			updatedHome.TemperatureControl.Heating = Enum.Parse<HeatingTypes>(Request.Form["ddlHeating"]);

            UpdateHome(updatedHome);
            TempData.Clear();
			HttpContext.Session.Remove("EditRooms");
			HttpContext.Session.Remove("EditImages");
			HttpContext.Session.Remove("EditAmenities");
			HttpContext.Session.Remove("EditUtilities");
			HttpContext.Session.Remove("EditTemperature");
			HttpContext.Session.Remove("EditHome");
			return RedirectToAction("AllEditHomes");
		}

        public async Task TryDeleteHome(int homeID)
        {
            try
            {
				string apiUrl = $"https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/DeleteHomeListing/{homeID}";
				using (HttpClient client = new HttpClient())
				{
					HttpResponseMessage response = await client.DeleteAsync(apiUrl);

					if (response.IsSuccessStatusCode)
					{
						string responseBody = await response.Content.ReadAsStringAsync();
						Console.WriteLine("Successfully deleted home listing.");
						Console.WriteLine("Response: " + responseBody);
					}
					else
					{
						Console.WriteLine($"HTTP Error: {response.StatusCode}");
						string errorBody = await response.Content.ReadAsStringAsync();
						Console.WriteLine("Error Details: " + errorBody);
					}
				}
			}
			catch (Exception ex)
			{
				// Handle other potential exceptions
				Console.WriteLine("An error occurred: " + ex.Message);
			}
        }

        public IActionResult DeleteHome(int home)
        {
            TryDeleteHome(home);
            return RedirectToAction("AllEditHomes", "RealEstateHome");
        }
	}
}
