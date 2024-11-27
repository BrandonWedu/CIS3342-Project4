using Microsoft.AspNetCore.Mvc;

namespace HomeListingAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ReadHomeController : Controller
	{
		[HttpPost("ReadHomes")]

		public Homes Post()
		{
			Homes allHomeListings = new Homes();
			allHomeListings = ReadHome.ReadAllHomes();

			return allHomeListings;
		}
	}
}
