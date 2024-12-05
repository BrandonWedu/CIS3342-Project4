using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4.Models;
using System.Net;

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

        public IActionResult ApplyFilter(string txtFilterCity)
        {
            return View();
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
