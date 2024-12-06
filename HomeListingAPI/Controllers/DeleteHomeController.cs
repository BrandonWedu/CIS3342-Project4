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

			WriteHome.DeleteHome(homeID);


			return Ok("Listing deleted");
		}
	}
}
