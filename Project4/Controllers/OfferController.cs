using Microsoft.AspNetCore.Mvc;
using Project4.Models;
using Project4.Models.ViewModels;

namespace Project4.Controllers
{
    //Handles Offer Create and Manage
    public class OfferController : Controller
    {
		private PropertyList allProperties = new PropertyList();
        private ReadOffers allOffers = new ReadOffers();
        private MakeOfferViewModel makeOfferModel = new MakeOfferViewModel();
        private WriteOffer newOffer = new WriteOffer();
        private WriteContingencies newContingencies = new WriteContingencies();
		public IActionResult MakeOffer(Home currentHome)
        {
            makeOfferModel.home = ReadHome.GetHomeByID((int)currentHome.HomeID);
            return View(makeOfferModel);
        }

        public IActionResult AddContingency(string contingency, MakeOfferViewModel model)
        {
            model.contingencies.Add(new Contingency(contingency));
            return View("MakeOffer", model);
        }

        public IActionResult RemoveContingency(int index, MakeOfferViewModel model)
        {
            model.contingencies.RemoveAtIndex(index);
            return View("MakeOffer", model);
        }

        public IActionResult AllOffers()
        {
            allOffers = new ReadOffers();
            return View(allOffers.GetAllOffers());
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
            ViewBag.Message = "Congraulations! Your offer was sucessfully placed!";
            model.offer.Home = model.home;
			newOffer.CreateNewOffer(model.offer);
            newContingencies.CreateNewContingencies(model.contingencies, ReadOffers.GetOfferIDByClientHomeID());
            return View("Confirmation");
        }
    }
}
