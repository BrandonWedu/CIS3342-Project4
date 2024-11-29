using Microsoft.AspNetCore.Mvc;

namespace HomeListingAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ReadHomeController : Controller
	{
		[HttpGet("ReadHomeListings")]

		public Homes Get()
		{
			Homes allHomeListings = new Homes();
			allHomeListings = ReadHome.ReadAllHomes();

			return allHomeListings;
		}
	}
}
