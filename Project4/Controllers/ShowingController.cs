using Microsoft.AspNetCore.Mvc;
using Project4.Models;
using Project4.Models.ViewModels;

namespace Project4.Controllers
{
    //Handles Showing Create and Manage
    public class ShowingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ScheduleShowing(ShowingViewModel vm)
        {
            Showing showing = new Showing(vm.Home, vm.Client, DateTime.Now, vm.Showing.ShowingTime, ShowingStatus.Pending);
            //call the model to do stuff with showing
            return View(vm);
        }
        public IActionResult ChangeStatus(int showingID, ShowingStatus showingStatus)
        {
            //use showingID and showing status to update the showing status
            return View();
        }
    }
}
