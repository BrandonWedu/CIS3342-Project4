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


		[HttpGet("ReadSingleHomeListing/{homeID}")]
		public Home Get(int homeID)
		{
			Homes allHomeListings = new Homes();
			allHomeListings = ReadHome.ReadAllHomes();
			Home selectedHome = null;
			foreach (Home currentHome in allHomeListings.List)
			{
				if (currentHome.HomeID == homeID)
				{
					selectedHome = currentHome;
				}
			}

			return selectedHome;
		}
	}
}
