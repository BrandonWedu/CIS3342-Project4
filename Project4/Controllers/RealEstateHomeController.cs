using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Project4.Models;
using System.Text.Json;

namespace Project4.Controllers
{
    //Handles Home Create, Modify and search
    public class RealEstateHomeController : Controller
    {
        [HttpGet]
        public IActionResult CreateHome()
        {
            if (HttpContext.Session.GetString("Agent") == null)
            {
                return View("Dashboard");
            }
            return View();
        }
       
        [HttpPost]
        public IActionResult HomeForm(string button)
        {
            int buttonNumber = button.Contains('_')? int.Parse(button.Split('_').Last()) : -1;
            switch (button.Split('_').First())
            {
                case "AddRoom":
                    AddRoom();
                    break;
                case "DeleteRoom":
                    DeleteRoom(buttonNumber);
                    break;
                case "AddUtility":
                    AddUtility();
                    break;
                case "DeleteUtility":
                    DeleteUtility(buttonNumber);
                    break;
                case "AddAmenity":
                    AddAmenity();
                    break;
                case "DeleteAmenity":
                    DeleteAmenity(buttonNumber);
                    break;
                case "AddImage":
                    AddImage();
                    break;
                case "DeleteImage":
                    DeleteImage(buttonNumber);
                    break;
                case "UploadImage":
                    UploadImage(buttonNumber);
                    break;
                case "AddHome":
                    AddHome();
                    break;

            }
            return View("CreateHome");
        }
        public void AddRoom() 
        {
            if (TempData["RoomCount"] == null)
            {
                TempData["RoomCount"] = 1;
            }
            TempData["RoomCount"] = (int)TempData["RoomCount"] + 1;
            RetainData();
        }
        public void DeleteRoom(int i)
        {
            TempData[$"RoomHidden_{i}"] = true;
            RetainData();
        }
        public void AddUtility() 
        {
            if (TempData["UtilityCount"] == null)
            {
                TempData["UtilityCount"] = 1;
            }
            TempData["UtilityCount"] = (int)TempData["UtilityCount"] + 1;
            RetainData();
        }
        public void DeleteUtility(int i)
        {
            TempData[$"UtilityHidden_{i}"] = true;
            RetainData();
        }
        public void AddAmenity() 
        {
            if (TempData["AmenityCount"] == null)
            {
                TempData["AmenityCount"] = 1;
            }
            TempData["AmenityCount"] = (int)TempData["AmenityCount"] + 1;
            RetainData();
        }
        public void DeleteAmenity(int i)
        {
            TempData[$"AmenityHidden_{i}"] = true;
            RetainData();
        }
        public void AddImage() 
        {
            if (TempData["ImageCount"] == null)
            {
                TempData["ImageCount"] = 1;
            }
            TempData["ImageCount"] = (int)TempData["ImageCount"] + 1;
            RetainData();
        }
        public void DeleteImage(int i)
        {
            TempData[$"ImageHidden_{i}"] = true;
            RetainData();
        }
        public void UploadImage(int i)
        {
            //Save Image To Server
            string here = Request.Form[$"fuImage_{i}"];
            IFormFile file = Request.Form.Files[$"fuImage_{i}"];
            if (file == null || file.FileName.Split('.').Last() != "png")
            {
                //error
                return;
            }

            //TODO: Modify Image Learning Opportunity
            //-------------------------------------------------------
            ModifyImage modifyImage = new ModifyImage();
            using(MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                modifyImage.Image = memoryStream.ToArray();
            }


            //Generate File Name
            string imageName = DateTime.Now.Ticks.ToString();

            //get the server path 
            string serverPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            string path = Path.Combine(serverPath, "FileStorage", imageName);

            using(FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                fileStream.Write(modifyImage.Image, 0, modifyImage.Image.Length);
            }
            TempData[$"ImageUploaded_{i}"] = true;
            RetainData();
        }
        public void AddHome() 
        { 
            //TODO: Code To Add Home to server through API
            string agentJson = HttpContext.Session.GetString("Agent");
            Agent agent = JsonSerializer.Deserialize<Agent>(agentJson);
            Home home = new Home(
                agent,
                0,
                new Address("", "", States.Alabama, ""),
                PropertyType.SingleFamily,
                DateTime.Now.Year,
                GarageType.SingleCar,
                "",
                DateTime.Now, 
                SaleStatus.OffMarket, 
                new Images(),
                new Amenities(), 
                new TemperatureControl(HeatingTypes.CentralHeating, CoolingTypes.CentralAir),
                new Rooms(), 
                new Utilities()
                );
        }

        //save request.form to temp data
        public void RetainData()
        {
            foreach (string key in Request.Form.Keys)
            {
                    TempData[key] = Request.Form[key];
            }
            TempData.Keep();
        }
    }
}
