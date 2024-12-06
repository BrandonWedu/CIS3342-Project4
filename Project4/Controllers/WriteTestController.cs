using Microsoft.AspNetCore.Mvc;

namespace Project4.Controllers
{
	public class WriteTestController : Controller
	{
		public IActionResult WriteTest()
		{
			return View();
		}

		public IActionResult SaveImage(IFormFile image)
		{
            string id = DateTime.Now.Ticks.ToString();
            string imageName = id + ".png";
            string relativePath = $"FileStorage/{id}"; // Relative path to the file
            string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath); // Absolute path to the file
			Console.WriteLine(absolutePath);

			if (!Directory.Exists(relativePath))
			{
				Directory.CreateDirectory(absolutePath);
			}

			if (image.Length > 0)
			{
				string fileExtension = Path.GetExtension(image.FileName);	
				string newFileName = $"{id}{fileExtension}";
				string fullPath = Path.Combine(absolutePath, newFileName);
				Console.WriteLine(fullPath);

				using (var fileStream = new FileStream(fullPath, FileMode.Create))
				{
					image.CopyTo(fileStream);
				}
			}

            return View("WriteTest");
		}
	}
}
