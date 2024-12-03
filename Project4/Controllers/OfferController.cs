using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4.Models;
using Project4.Models.ViewModels;
using System.Reflection;

namespace Project4.Controllers
{
    //Handles Offer Create and Manage
    public class OfferController : Controller
    {
		[HttpPost]        
		
		public IActionResult MakeOffer(int homeID)
        {
            if (homeID > 0)
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
				string seralizedHome = JsonConvert.SerializeObject(currentHome);
				HttpContext.Session.SetString("CurrentHome", seralizedHome);

				if (HttpContext.Session.GetString("OfferContingencies") == null)
                {
                    List<string> currentContingencies = new List<string>();
                    string seralizedContingencies = JsonConvert.SerializeObject(currentContingencies);
                    HttpContext.Session.SetString("OfferContingencies", seralizedContingencies);
                    ViewBag.OfferContingencies = currentContingencies;
                    return View();
                }
                else
                {
                    string seralizedContingencies = HttpContext.Session.GetString("OfferContingencies");
                    List<string> currentContingencies = JsonConvert.DeserializeObject<List<string>>(seralizedContingencies);
                    ViewBag.OfferContingencies = currentContingencies;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

		}
		[HttpPost]
        public IActionResult AddContingency(string newContingency)
        {
            if (newContingency == "")
            {
                ViewBag.OfferError = "Contingency cannot be empty.";
            }
            else
            {
                string seralizedContingencies = HttpContext.Session.GetString("OfferContingencies");
                List<string> currentContingencies = JsonConvert.DeserializeObject<List<string>>(seralizedContingencies);
                currentContingencies.Add(newContingency);
                seralizedContingencies = JsonConvert.SerializeObject(currentContingencies);
                HttpContext.Session.SetString("OfferContingencies", seralizedContingencies);
                ViewBag.OfferContingencies = currentContingencies;
            }
            return View("MakeOffer");

		}
		[HttpPost]
        public IActionResult RemoveContingency(string removedContingency)
        {
            string seralizedContingencies = HttpContext.Session.GetString("OfferContingencies");
            List<string> currentContingencies = JsonConvert.DeserializeObject<List<string>>(seralizedContingencies);

            if (currentContingencies.Contains(removedContingency))
            {
                currentContingencies.Remove(removedContingency);
            }
            seralizedContingencies = JsonConvert.SerializeObject(currentContingencies);
            HttpContext.Session.SetString("OfferContingencies", seralizedContingencies);
            ViewBag.OfferContingencies = currentContingencies;

            return View("MakeOffer");
        }

        public IActionResult AllOffers()
        {
            string agentCookie = HttpContext.Request.Cookies["LoggedInAgent"];
            Agent currentAgent = JsonConvert.DeserializeObject<Agent>(agentCookie);
            TempData["Agent"] = agentCookie;
            return View();
        }


        public IActionResult FinalizeOffer()
        {
            string firstName = Request.Form["FirstName"];
            string lastName = Request.Form["LastName"];
            string email = Request.Form["Email"];
            string phone = Request.Form["Phone"];
            string offerAmount = Request.Form["OfferAmount"];
            string saleType = Request.Form["SaleType"];
            string sellHome = Request.Form["SellHomePrior"];
            string moveInDate = Request.Form["MoveInDate"];
            string clientAddress = Request.Form["clientAddress"];
            string clientCity = Request.Form["clientCity"];
            string clientState = Request.Form["clientState"];
            string clientZip = Request.Form["clientZip"];

            string serializedHome = HttpContext.Session.GetString("CurrentHome");
            Home home = JsonConvert.DeserializeObject<Home>(serializedHome);
            States stateEnum = Enum.Parse<States>(clientState);
            Address newAddress = new Address(clientAddress, clientCity, stateEnum, clientZip);
            Client newClient = new Client(firstName, lastName, newAddress, phone, email);
            int clientID = WriteClient.CreateNew(newClient);
            Client actualClient = ReadClients.GetClientByLastNameAndAddress(newClient);
            Offer newOffer = new Offer(home, actualClient, int.Parse(offerAmount), Enum.Parse<TypeOfSale>(saleType), bool.Parse(sellHome), DateTime.Parse(moveInDate), OfferStatus.Pending);
            
            int offerID = WriteOffer.CreateNew(newOffer);
            // Read Offers will need updated to use the HOME api call
            Offer actualOffer = ReadOffers.GetOfferByHomeClientAmount(home, actualClient, int.Parse(offerAmount));

            string seralizedContingencies = HttpContext.Session.GetString("OfferContingencies");
            List<string> currentContingencies = JsonConvert.DeserializeObject<List<string>>(seralizedContingencies);

            Contingencies newContingencies = new Contingencies();
            foreach (string contingency in currentContingencies)
            {
                newContingencies.Add(new Contingency(actualOffer.OfferID, contingency));
            }
            WriteContingencies.CreateNew(newContingencies);


            TempData["Message"] = "Congratulations! Your offer was successfully placed!";
            TempData["FirstName"] = actualOffer.Client.FirstName;
            TempData["LastName"] = actualOffer.Client.LastName;
            TempData["OfferHomeAddress"] = actualOffer.Home.Address.Street + ", " + actualOffer.Home.Address.City + "," + actualOffer.Home.Address.State + ", " + actualOffer.Home.Address.ZipCode;
            TempData["OfferAmount"] = actualOffer.Amount;


            HttpContext.Session.Remove("OfferContingencies");
            HttpContext.Session.Remove("CurrentHome");

            return RedirectToAction("Confirmation");
        }

        public async Task<IActionResult> Confirmation()
        {
            return View("Confirmation");

        }

        public IActionResult AcceptOffer(int offerID)
        {
            WriteOffer.UpdateOfferStatus(offerID, OfferStatus.Accepted);
            return View("AllOffers");
        }

        public IActionResult DenyOffer(int offerID)
        {
			WriteOffer.UpdateOfferStatus(offerID, OfferStatus.Rejected);
			return View("AllOffers");
		}




    }
}
