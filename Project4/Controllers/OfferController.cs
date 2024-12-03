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
            ViewBag.FirstName = Request.Form["FirstName"];
            ViewBag.LastName = Request.Form["LastName"];
            ViewBag.Email = Request.Form["Email"];
            ViewBag.Phone = Request.Form["Phone"];
            ViewBag.ClientAddress = Request.Form["clientAddress"];
            ViewBag.ClientCity = Request.Form["clientCity"];
            ViewBag.ClientZip = Request.Form["clientZip"];
            ViewBag.OfferAmount = Request.Form["OfferAmount"];
            ViewBag.MoveInDate = Request.Form["MoveInDate"];
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
            ViewBag.FirstName = Request.Form["FirstName"];
            ViewBag.LastName = Request.Form["LastName"];
            ViewBag.Email = Request.Form["Email"];
            ViewBag.Phone = Request.Form["Phone"];
            ViewBag.ClientAddress = Request.Form["clientAddress"];
            ViewBag.ClientCity = Request.Form["clientCity"];
            ViewBag.ClientZip = Request.Form["clientZip"];
            ViewBag.OfferAmount = Request.Form["OfferAmount"];
            ViewBag.MoveInDate = Request.Form["MoveInDate"];
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
            string agentSession = HttpContext.Session.GetString("Agent");
            Agent currentAgent = JsonConvert.DeserializeObject<Agent>(agentSession);
            ViewBag.Agent = currentAgent;
            return View("AllOffers");
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
            List<string> inputs = new List<string>();
            inputs.Add(firstName);
            inputs.Add(lastName);
            inputs.Add(email);
            inputs.Add(phone);
            inputs.Add(offerAmount);
            inputs.Add(sellHome);
            inputs.Add(moveInDate);
            inputs.Add(clientAddress);
            inputs.Add(clientCity);
            inputs.Add(clientZip);
            
            if (ValidateOffer(inputs) == false)
            {
                ViewBag.OfferError = "Please fix errors below and resubmit the offer!";
                ViewBag.FirstName = firstName;
                ViewBag.LastName = lastName;
                ViewBag.Email = email;
                ViewBag.Phone = phone;
                ViewBag.OfferAmount = offerAmount;
                ViewBag.MoveInDate = moveInDate;
                ViewBag.ClientAddress = clientAddress;
                ViewBag.ClientCity = clientCity;
                ViewBag.ClientZip = clientZip;
                return View("MakeOffer");
            }
            else
            {
                string serializedHome = HttpContext.Session.GetString("CurrentHome");
                Home home = JsonConvert.DeserializeObject<Home>(serializedHome);
                States stateEnum = Enum.Parse<States>(clientState);
                Address newAddress = new Address(clientAddress, clientCity, stateEnum, clientZip);
                Client newClient = new Client(firstName, lastName, newAddress, phone, email);
                int clientID = WriteClient.CreateNew(newClient);
                Client actualClient = ReadClients.GetClientByLastNameAndAddress(newClient);
                Offer newOffer = new Offer(home, actualClient, int.Parse(offerAmount), Enum.Parse<TypeOfSale>(saleType), bool.Parse(sellHome), DateTime.Parse(moveInDate), OfferStatus.Pending);

                int offerID = WriteOffer.CreateNew(newOffer);
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


        }

        private bool ValidateOffer(List<string> inputs)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(inputs[0]))
            {
                isValid = false;
                ViewBag.FnameError = "First Name Is Requried!";
            }
            if (string.IsNullOrEmpty(inputs[1]))
            {
                isValid = false;
                ViewBag.LnameError = "Last Name Is Requried!";
            }
            if (string.IsNullOrEmpty(inputs[2]))
            {
                isValid = false;
                ViewBag.EmailError = "Email Is Requried!";
            }
            if (string.IsNullOrEmpty(inputs[3]))
            {
                isValid = false;
                ViewBag.PhoneError = "Phone Number Is Requried!";
            }
            if (string.IsNullOrEmpty(inputs[4]) || int.TryParse(inputs[4], out _ ) == false)
            {
                isValid = false;
                ViewBag.AmountError = "Offer Amount Is Requried!";
            }
            if (string.IsNullOrEmpty(inputs[5]))
            {
                isValid = false;
                ViewBag.HomeError = "Current Home Sale Is Requried!";
            }
            if (string.IsNullOrEmpty(inputs[6]) || DateTime.TryParse(inputs[6], out _) == false)
            {
                isValid = false;
                ViewBag.DateError = "Move In Date Is Requried!";
            }
            if (string.IsNullOrEmpty(inputs[7]))
            {
                isValid = false;
                ViewBag.StreetError = "Street Address Sale Is Requried!";
            }
            if (string.IsNullOrEmpty(inputs[8]))
            {
                isValid = false;
                ViewBag.CityError = "City Sale Is Requried!";
            }
            if (string.IsNullOrEmpty(inputs[9]))
            {
                isValid = false;
                ViewBag.ZipError = "Zip Code Sale Is Requried!";
            }


            return isValid;
        }
        public async Task<IActionResult> Confirmation()
        {
            return View("Confirmation");

        }

        public IActionResult AcceptOffer(int offerID)
        {
            WriteOffer.UpdateOfferStatus(offerID, OfferStatus.Accepted);
			string agentSession = HttpContext.Session.GetString("Agent");
			Agent currentAgent = JsonConvert.DeserializeObject<Agent>(agentSession);
			ViewBag.Agent = currentAgent;
			return View("AllOffers");
        }

        public IActionResult DenyOffer(int offerID)
        {
			WriteOffer.UpdateOfferStatus(offerID, OfferStatus.Rejected);
			string agentSession = HttpContext.Session.GetString("Agent");
			Agent currentAgent = JsonConvert.DeserializeObject<Agent>(agentSession);
			ViewBag.Agent = currentAgent;
			return View("AllOffers");
		}




    }
}
