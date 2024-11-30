using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4.Models;
using Project4.Models.ViewModels;

namespace Project4.Controllers
{
    //Handles Offer Create and Manage
    public class OfferController : Controller
    {
        //private Contingencies offerContingencies = new Contingencies();
		public IActionResult MakeOffer(MakeOfferViewModel model)
        {
			if (TempData["OfferContingencies"] != null)
			{
				string serializedContingencies = TempData["OfferContingencies"].ToString();
				model.offerContingencies = JsonConvert.DeserializeObject<Contingencies>(serializedContingencies);

				// Reassign TempData to persist the value for subsequent requests
				TempData["OfferContingencies"] = serializedContingencies;
			}
			else
			{
				// Initialize empty contingencies if TempData is null
				Contingencies contingencies = new Contingencies();
				TempData["OfferContingencies"] = JsonConvert.SerializeObject(contingencies);
			}

			return View(model);

		}
		[HttpPost]
        public IActionResult AddContingency(MakeOfferViewModel model)
        {
			// Retrieve contingencies from TempData
			string serializedContingencies = TempData["OfferContingencies"] as string;
			Contingencies offerContingencies = string.IsNullOrEmpty(serializedContingencies)
				? new Contingencies() // Initialize if TempData is empty
				: JsonConvert.DeserializeObject<Contingencies>(serializedContingencies);

			// Add the new contingency
			if (!string.IsNullOrEmpty(model.NewContingency))
			{
				offerContingencies.Add(new Contingency(model.NewContingency));
			}

			// Clear the input field
			model.NewContingency = "";
			model.offerContingencies = offerContingencies;

			// Save updated contingencies back to TempData
			TempData["OfferContingencies"] = JsonConvert.SerializeObject(offerContingencies);

			return RedirectToAction("MakeOffer", model);
		}
		[HttpPost]
        public IActionResult RemoveContingency(MakeOfferViewModel model, string removedContingency)
        {

			// Retrieve contingencies from TempData
			string serializedContingencies = TempData["OfferContingencies"] as string;
			Contingencies offerContingencies = string.IsNullOrEmpty(serializedContingencies)
				? new Contingencies() // Initialize if TempData is empty
				: JsonConvert.DeserializeObject<Contingencies>(serializedContingencies);

			// Remove the specified contingency
			offerContingencies.List.RemoveAll(c => c.OfferContingency == removedContingency);

			// Save updated contingencies back to TempData
			TempData["OfferContingencies"] = JsonConvert.SerializeObject(offerContingencies);

			return RedirectToAction("MakeOffer", model);
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
