using Microsoft.AspNetCore.Mvc;
using Project4.Models;
using Project4.Models.ViewModels;
using System.Text.Json;

namespace Project4.Controllers
{
    //Handles Showing Create and Manage
    public class ShowingController : Controller
    {
        public IActionResult ViewShowings(ViewShowingsViewModel model)
        {
            string agentJson = HttpContext.Session.GetString("Agent");
            if (agentJson == null)
            {
                return View("Dashboard");
            }
            Agent agent = JsonSerializer.Deserialize<Agent>(agentJson);
            model.Showings = ReadShowing.GetShowingsByAgentID(agent.AgentID);
            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int showingID, ShowingStatus showingStatus)
        {
            if(WriteShowing.UpdateShowingStatusByShowingID(showingID, showingStatus))
            {
                TempData["SuccessMessage"] = $"{showingID} status updated to {showingStatus.ToString()}";
            }
            else
            {
                TempData["ErrorMessage"] = $"{showingID} status NOT updated";
            }
            return View("ViewShowings", new ViewShowingsViewModel());
        }

        public IActionResult ScheduleShowing(ScheduleShowingsViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult ShowingRequest(Agent? agent, Showing showing)
        {
            if(WriteShowing.Create(showing))
            {
                HomeProfileViewModel model = new HomeProfileViewModel();
                model.Agent = agent;
                return View("HomeProfile", model);
            } else
            {
                ScheduleShowingsViewModel model = new ScheduleShowingsViewModel();
                model.Agent = agent;
                model.Home = showing.Home;
                //make error text
                return View("ScheduleShowing", model);
            }
        }
    }
}
