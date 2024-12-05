using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4.Models;
using Project4.Models.ViewModels;
using System.Text.Json;

namespace Project4.Controllers
{
    //Handles Showing Create and Manage
    public class ShowingController : Controller
    {
        public IActionResult ViewShowings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangeStatus()
        {
            //if(WriteShowing.UpdateStatus(showingID, showingStatus))
            //{
            //    TempData["SuccessMessage"] = $"{showingID} status updated to {showingStatus.ToString()}";
            //}
            //else
            //{
            //    TempData["ErrorMessage"] = $"{showingID} status NOT updated";
            //}
            return View("ViewShowings");
        }

        public IActionResult ScheduleShowing(Home home)
        {

            return View("ScheduleShowing", home);
        }

        [HttpPost]
        public IActionResult ShowingRequest()
        {
            Home home = (Home)TempData["Home"];
            Client client = new Client(
                    "firstname",
                    "lastname",
                    new Address(),
                    "1231231234",
                    "tui7895@temple.edu"
                );
            //Showing showing = new Showing(
                //get home
              //  client,
                //DateTime.Now,
                //get showing time
                //ShowingStatus.Pending
                //);

         //   if(WriteShowing.CreateNew(showing))
          //  {
                //HomeProfileModel model = new HomeProfileModel();
                //model.Home = showing.Home;
                //return View("HomeProfile", model);
            //} else
        //    {
        //        ScheduleShowingsViewModel model = new ScheduleShowingsViewModel();
                //model.Home = showing.Home;
                //make error text
        //        return View("ScheduleShowing", model);
           // }
            return View();
        }
    }
}
