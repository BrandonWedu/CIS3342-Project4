using Microsoft.AspNetCore.Mvc;

namespace HomeListingAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UpdateHomeController : Controller
	{
		[HttpPut("UpdateHomeListing")]
		public IActionResult Put([FromBody] Home updatedHome)
		{
			if (updatedHome == null)
			{
				return BadRequest("No Home Listing Provided In Call");
			}

			Home currentHome = ReadHome.GetHomeByHomeID((int)updatedHome.HomeID);

			if (currentHome == null)
			{
				return NotFound($"Home listing ID {updatedHome.HomeID} Was Not Found!");
			}


			currentHome.Address = updatedHome.Address;
			currentHome.Rooms = updatedHome.Rooms;
			currentHome.Amenities = updatedHome.Amenities;
			currentHome.Images = updatedHome.Images;
			currentHome.Cost = updatedHome.Cost;
			currentHome.Description = updatedHome.Description;
			currentHome.PropertyType = updatedHome.PropertyType;
			currentHome.SaleStatus = updatedHome.SaleStatus;
			currentHome.GarageType = updatedHome.GarageType;
			currentHome.TemperatureControl = updatedHome.TemperatureControl;
			currentHome.YearConstructed = updatedHome.YearConstructed;
			currentHome.Utilities = updatedHome.Utilities;

			Home oldHome = ReadHome.GetHomeByHomeID((int)updatedHome.HomeID);

			WriteHome.UpdateHome(currentHome, oldHome);
			return Ok("Home Listing Updated");
		}
	}
}
