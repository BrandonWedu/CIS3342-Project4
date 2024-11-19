using Microsoft.AspNetCore.Mvc;
using Project4.Models;
using Project4.Models.ViewModels;

namespace Project4.Controllers
{
    //Handles Showing Create and Manage
    public class ShowingController : Controller
    {
        public IActionResult ViewShowings(ViewShowingsViewModel model)
        {
            if (model.Agent == null)
            {
                return View("Dashboard");
            }
            if (model.Showings == null)
            {
                model.Showings = ReadShowing.GetShowingsByAgentID(agent.AgentID);
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult ChangeStatus(Agent agent, int showingID, ShowingStatus showingStatus)
        {
            if(WriteShowing.UpdateShowingStatusByShowingID(showingID, showingStatus))
            {
                TempData["SuccessMessage"] = $"{showingID} status updated to {showingStatus.ToString()}";
            }
            else
            {
                TempData["ErrorMessage"] = $"{showingID} status NOT updated";
            }
            ViewShowingsViewModel model = new ViewShowingsViewModel();
            model.Agent = agent;
            return View("ViewShowings", model);
        }
        public IActionResult ScheduleShowing(Agent agent, Home home)
        {
            ViewShowingsViewModel model = new ViewShowingsViewModel();
            if (agent != null)
            {
                model.Agent = agent;
            }
            model.Home = home;
            return View(model);
        }
        public IActionResult ScheduleShowing(ViewShowingsViewModel model)
        {
            View("HomeProfile", mogent, HomeID);
        }
    }
}
