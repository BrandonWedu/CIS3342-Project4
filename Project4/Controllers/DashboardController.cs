using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4.Models;

namespace Project4.Controllers
{
    public class DashboardController : Controller
    {
		//private PropertyList allProperties = new PropertyList();
		public IActionResult Dashboard()
		{
			string apiUrl = "https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadHomeListings";
			HttpClient client = new HttpClient();
			HttpResponseMessage response = client.GetAsync(apiUrl).Result;

			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine("API Request Failed: " + response.StatusCode);
				ViewBag.Homes = null;
				return View();
			}

			string jsonString = response.Content.ReadAsStringAsync().Result;

			Homes allHomes = JsonConvert.DeserializeObject<Homes>(jsonString);

			if (allHomes == null || allHomes.List == null || allHomes.List.Count == 0)
			{
				Console.WriteLine("Deserialization Failed or No Homes Returned");
				ViewBag.Homes = null;
				return View();
			}

			foreach (Home home in allHomes.List)
			{
				home.Address ??= new Address();
				home.Images ??= new Images();
				home.Amenities ??= new Amenities();
			}

			ViewBag.Homes = allHomes;


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

        public IActionResult ViewDetail(int homeID)
        {
			string apiUrl = $"https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadSingleHomeListing/{homeID}";
			HttpClient client = new HttpClient();
			HttpResponseMessage response = client.GetAsync(apiUrl).Result;

			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine($"API Request Failed. Status Code: {response.StatusCode}");
				ViewBag.Home = null;
				return View(); // Return the view with no data
			}


			string jsonString = response.Content.ReadAsStringAsync().Result;


			Home currentHome = JsonConvert.DeserializeObject<Home>(jsonString);

			if (currentHome == null)
			{
				Console.WriteLine("Deserialization Failed: The Home object is null.");
				ViewBag.Home = null;
				return View();
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
			return RedirectToAction("ScheduleShowing", "Showing", currentHome);
		}
    }
}
