using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4.Models;
using Project4.Models.ViewModels;

namespace Project4.Controllers
{
    //Handles Offer Create and Manage
    public class OfferController : Controller
    {
		[HttpGet]        
		
		public IActionResult MakeOffer()
        {
			if (HttpContext.Session.GetString("OfferContingencies") == null)
			{
				List<string> currentContingencies = new List<string>();
				string seralizedContingencies = JsonConvert.SerializeObject(currentContingencies);
				HttpContext.Session.SetString("OfferContingencies", seralizedContingencies);
				ViewBag.OfferContingencies = currentContingencies;
			}
			else
			{
				string seralizedContingencies = HttpContext.Session.GetString("OfferContingencies");
                List<string> currentContingencies = JsonConvert.DeserializeObject<List<string>>(seralizedContingencies);
                ViewBag.OfferContingencies = currentContingencies;
            }

			return View();

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
            return View();
            //allOffers = new ReadOffers();
            //return View(allOffers.GetAllOffers());
        }

        public IActionResult UpdateOffer(Offer currentOffer, OfferStatus newStatus)
        {

            return View();
        }

        public IActionResult FinalizeOffer(MakeOfferViewModel model)
        {

            //Create a new client with info from the form
            //Get the clientID from the newly created client
            //Create a new offer with info from the form
            //Get the offerID from the newly created offer
            //Create any contingencies with info from the model
            //ViewBag.Message = "Congraulations! Your offer was sucessfully placed!";
            //model.offer.Home = model.home;
			//newOffer.CreateNewOffer(model.offer);
            //newContingencies.CreateNewContingencies(model.contingencies, ReadOffers.GetOfferIDByClientHomeID());
            return View("Confirmation");
        }
    }
}
