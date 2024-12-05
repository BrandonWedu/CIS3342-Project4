using Microsoft.AspNetCore.Mvc;

namespace HomeListingAPI.Controllers
{

	public class DeleteHomeController : Controller
	{
		[HttpDelete]
		[Route("DeleteHomeListing/{homeID}")]
		public IActionResult Delete(int homeID)
		{
			return View();
		}
	}
}
