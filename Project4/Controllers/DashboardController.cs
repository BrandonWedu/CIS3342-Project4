using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4.Models;
using System.Net;
using System.Text.RegularExpressions;

namespace Project4.Controllers
{
    public class DashboardController : Controller
    {

        public IActionResult Dashboard()
        {
            CleanUpSession();
            string apiUrl = "https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadHomeListings";

            WebRequest request = WebRequest.Create(apiUrl);
            WebResponse resposne = request.GetResponse();
            Stream dataStream = resposne.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string data = reader.ReadToEnd();
            reader.Close();
            resposne.Close();
            Homes allHomes = JsonConvert.DeserializeObject<Homes>(data);
            ViewBag.Homes = allHomes;
            return View();
        }

        public IActionResult PreviousImage()
        {
            int totalImageCount = int.Parse(HttpContext.Session.GetString("TotalImageCount"));
            int currentImage = int.Parse(HttpContext.Session.GetString("CurrentImage"));

            if (currentImage >= 1)
            {
                currentImage--;
            }
            HttpContext.Session.SetString("CurrentImage", currentImage.ToString());
            return RedirectToAction("ViewDetail");
        }

        public IActionResult NextImage()
        {
            int totalImageCount = int.Parse(HttpContext.Session.GetString("TotalImageCount"));
            int currentImage = int.Parse(HttpContext.Session.GetString("CurrentImage"));
            if (currentImage < (totalImageCount - 1))
            {
                currentImage++;
            }
            HttpContext.Session.SetString("CurrentImage", currentImage.ToString());
            return RedirectToAction("ViewDetail");
        }

        //Search functionality
        public IActionResult ApplyFilter()
        {
            //Query API to get a list of all homes
            string apiUrl = "https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadHomeListings";
            WebRequest request = WebRequest.Create(apiUrl);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string data = reader.ReadToEnd();
            reader.Close();
            response.Close();
            Homes allHomes = JsonConvert.DeserializeObject<Homes>(data);
            Homes listOfHomes = allHomes;

            // Filterable Values
			string? street = string.IsNullOrWhiteSpace(Request.Form["txtFilterStreet"]) ? null : (string?)Request.Form["txtFilterStreet"];
			string? city = string.IsNullOrWhiteSpace(Request.Form["txtFilterCity"]) ? null : (string?)Request.Form["txtFilterCity"];
			string? zipCode = string.IsNullOrWhiteSpace(Request.Form["txtFilterZipCode"]) ? null : (string?)Request.Form["txtFilterZipCode"];
			int? priceMin = int.TryParse(Request.Form["txtFilterMinPrice"], out var parsedPriceMin) ? parsedPriceMin : null;
			int? priceMax = int.TryParse(Request.Form["txtFilterMaxPrice"], out var parsedPriceMax) ? parsedPriceMax : null;
			int? houseSizeMin = int.TryParse(Request.Form["txtFilterMinHouseSize"], out var parsedHouseSizeMin) ? parsedHouseSizeMin : null;
			int? houseSizeMax = int.TryParse(Request.Form["txtFilterMaxHouseSize"], out var parsedHouseSizeMax) ? parsedHouseSizeMax : null;
			int? bedroomMin = int.TryParse(Request.Form["txtMinBedroom"], out var parsedBedroomMin) ? parsedBedroomMin : null;
			double? bathroomMin = double.TryParse(Request.Form["txtMinBathroom"], out var parsedBathroomMin) ? parsedBathroomMin : null;


			States? state = Request.Form["ddlFilterState"] == "Select a State"? null : Enum.Parse<States>(Request.Form["ddlFilterState"]);
			SaleStatus? saleStatus = Request.Form["ddlFilterSaleStatus"] == "Select a Sale Status"? null : Enum.Parse<SaleStatus>(Request.Form["ddlFilterSaleStatus"]); 

            PropertyType? propertyType =  string.IsNullOrWhiteSpace(Request.Form["radFilterPropertyType"]) ? null : Enum.Parse<PropertyType>(Request.Form["radFilterPropertyType"]);

			List<AmenityType> amenities = new List<AmenityType>();
			foreach (string key in Request.Form.Keys)
			{
				if (key.StartsWith("chkFilterAmenities_"))
				{
					string amenityValue = key.Replace("chkFilterAmenities_", ""); // Get the AmenityType name from the key
					if (Enum.TryParse(amenityValue, out AmenityType selectedAmenity))
					{
						amenities.Add(selectedAmenity); // Add to the list if the parsing is successful
					}
				}
			}

			Homes filteredHomes = SearchHomes.Search(listOfHomes, street, city, state, zipCode, priceMin, priceMax, propertyType, houseSizeMin, houseSizeMax, bedroomMin, bathroomMin, amenities, saleStatus);
            ViewBag.Homes = filteredHomes;
            RetainData();

			return View("Dashboard");
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


        public IActionResult ClearFilter()
        {
            return View();
        }

        public void CleanUpSession()
        {
            HttpContext.Session.Remove("CurrentImage");
            HttpContext.Session.Remove("CurrentViewHome");
            HttpContext.Session.Remove("TotalImageCount");
            HttpContext.Session.Remove("ScheduleShowing");
            HttpContext.Session.Remove("CurrentHome");
            HttpContext.Session.Remove("OfferContingencies");
        }

        public IActionResult ViewDetail(int? homeID)
        {
            Home currentHome;
            if (homeID.HasValue)
            {
                string apiUrl = $"https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadSingleHomeListing/{homeID}";
                WebRequest request = WebRequest.Create(apiUrl);
                WebResponse resposne = request.GetResponse();
                Stream dataStream = resposne.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string data = reader.ReadToEnd();
                reader.Close();
                resposne.Close();
                currentHome = JsonConvert.DeserializeObject<Home>(data);
                HttpContext.Session.SetString("CurrentViewHome", data);
                HttpContext.Session.SetString("CurrentImage", "0");
                HttpContext.Session.SetString("TotalImageCount", currentHome.Images.List.Count.ToString());
                ViewBag.CurrentImage = 0;

            }
            else
            {
                string homeData = HttpContext.Session.GetString("CurrentViewHome");
                currentHome = JsonConvert.DeserializeObject<Home>(homeData);
                int currentImage = int.Parse(HttpContext.Session.GetString("CurrentImage"));
                ViewBag.CurrentImage = currentImage;
            }
            ViewBag.Home = currentHome;
            return View();
        }

        public IActionResult PassShowing(int homeID)
        {
            string apiUrl = $"https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadSingleHomeListing/{homeID}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            string jsonString = response.Content.ReadAsStringAsync().Result;
            Home currentHome = JsonConvert.DeserializeObject<Home>(jsonString);
            HttpContext.Session.SetString("ShowingHome", jsonString);
            return RedirectToAction("ScheduleShowing", "Showing");
        }
    }
}
