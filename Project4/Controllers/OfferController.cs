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
		[HttpGet]        
		
		public IActionResult MakeOffer(int homeID)
        {
            if (homeID > 0)
            {
                //API call to get the home by ID
                HttpClient client = new HttpClient();
                string apiURL = "";
                HttpContent content = new StringContent(homeID.ToString());
                Task response = client.PostAsync(apiURL, content);
                //string responseBody = response.Content.ReadAsStringAsync().Result;
                //Home currentHome = JsonConvert.DeseralizeObject<Home>(responseBody)
                //string seralizedHome = JsonConvert.SerializeObject(currentHome);
                //HttpContext.Session.SetString("CurrentHome", serializedHome);

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
            string clientAddress = Request.Form["Address"];
            string clientCity = Request.Form["City"];
            string clientState = Request.Form["State"];
            string clientZip = Request.Form["Zip"];
            string serializedHome = HttpContext.Session.GetString("CurrentHome");
            Home home = JsonConvert.DeserializeObject<Home>(serializedHome);
            Address newAddress = new Address(clientAddress, clientCity, Enum.Parse<States>(clientState), clientZip);
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
            TempData["OfferHomeAddress"] = actualOffer.Home.Address;
            TempData["OfferAmount"] = actualOffer.Amount;


            HttpContext.Session.Remove("OfferContingencies");
            HttpContext.Session.Remove("CurrentHome");

            return RedirectToAction("Confirmation");
        }

        public async Task<IActionResult> Confirmation()
        {
            await Task.Delay(6000);

            return RedirectToAction("Dashboard", "Dashboard");

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
