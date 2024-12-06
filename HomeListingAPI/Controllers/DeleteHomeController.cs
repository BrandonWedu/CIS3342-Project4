using Microsoft.AspNetCore.Mvc;

namespace HomeListingAPI.Controllers
{

	public class DeleteHomeController : Controller
	{
		[HttpDelete("DeleteHomeListing/{homeID}")]
		public IActionResult Delete(int homeID)
		{
			Home currentHome = ReadHome.GetHomeByHomeID(homeID);
			if (currentHome == null)
			{
				return NotFound("Home Listing Could Not Be Found");
			}

			WriteRoom.DeleteRoom(homeID);
			WriteHomeImage.DeleteHomeImage(homeID);
			WriteAmenity.DeleteAmenity(homeID);
			WriteUtility.DeleteUtility(homeID);
			WriteTemperatureControl.DeleteTemperatureControl(homeID);
			WriteOffers.DeleteOffer(homeID);
			WriteShowings.DeleteShowing(homeID);
			WriteHome.DeleteHome(homeID);


			return Ok("Listing deleted");
		}
	}
}
