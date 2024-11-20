using Microsoft.AspNetCore.Mvc;
using Project4.Models;

namespace Project4.Controllers
{
    //Handles Offer Create and Manage
    public class OfferController : Controller
    {
		private PropertyList allProperties = new PropertyList();
        private ReadOffers allOffers = new ReadOffers();
		public IActionResult MakeOffer(int propertyID)
        {
            Property currentProperty = allProperties.GetPropertyByPropertyID(propertyID);
            return View(currentProperty);
        }

        public IActionResult ViewOffer(int offerID)
        {
            allOffers = new ReadOffers();
            OfferList offers = allOffers.GetOfferByOfferID(offerID);
            return View(offers);
        }

        public IActionResult AllOffers()
        {
            allOffers = new ReadOffers();
            return View(allOffers.GetAllOffers());
        }

        public IActionResult UpdateOffer(int offerID, OfferStatus newStatus)
        {

            return View();
        }
    }
}
