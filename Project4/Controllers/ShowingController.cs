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
        [HttpPost]
        public IActionResult ViewShowings()
        {
            if (HttpContext.Session.GetString("Agent") != null)
            {
                string agentJson = HttpContext.Session.GetString("Agent");
                Agent agent = System.Text.Json.JsonSerializer.Deserialize<Agent>(agentJson);
                Showings showings = ReadShowing.GetShowingsByAgentID(agent.AgentID);
                TempData["Showings"] = showings;
                return View();
            }
            //Send to Error Page
            return RedirectErrorConfirm(false, new List<string>
            {
                $"You are not signed in as an Agent"
            });
        }

        [HttpPost]
        public IActionResult ChangeStatus()
        {
            try
            {
                if (HttpContext.Session.GetString("Agent") == null) 
                {
                    //Send to Error Page
                    return RedirectErrorConfirm(false, new List<string>
                    {
                        $"You are not signed in as an Agent"
                    });
                }
                int showingID = int.Parse(Request.Form["ShowingID"].ToString());
                ShowingStatus showingStatus = (ShowingStatus)Enum.Parse(typeof(ShowingStatus), Request.Form["ddlShowingStatus"].ToString());
                ShowingStatus originalShowingStatus = (ShowingStatus)Enum.Parse(typeof(ShowingStatus), TempData["OriginalShowingStatus"].ToString());
                if(showingStatus == originalShowingStatus)
                {
                    //Send to Error Page
                    return RedirectErrorConfirm(false, new List<string>
                    {
                        $"ShowingID: {showingID} is already Status: {showingStatus}"
                    });
                }
                else if(WriteShowing.UpdateStatus(showingID, showingStatus))
                {
                    return RedirectErrorConfirm(true, new List<string>
                    {
                        $"ShowingID: {showingID} status updated to Status: {showingStatus.ToString()}"
                    });
                }
                else
                {
                    return RedirectErrorConfirm(false, new List<string>
                    {
                        $"{showingID} status NOT updated"
                    });
                }
                //Send to confirmation page
                return RedirectErrorConfirm(true, new List<string>
                {
                    $"Confirmation Message: ShowingID: {showingID} status updated to Status: {showingStatus.ToString()}"
                });
            }
            catch (Exception ex)
            {
                //Send to Error Page
                return RedirectErrorConfirm(false, new List<string>
                {
                    $"Error Message: {ex}"
                });
            }
        }

        public IActionResult ScheduleShowing()
        {
            string homeJson = HttpContext.Session.GetString("ShowingHome");
            Home showingHome = JsonConvert.DeserializeObject<Home>(homeJson);
            TempData["Home"] = showingHome;
            return View();
        }

        [HttpPost]
        public IActionResult ShowingRequest()
        {
            string homeJson = HttpContext.Session.GetString("ShowingHome");
            Home home = JsonConvert.DeserializeObject<Home>(homeJson);
            string firstName = Request.Form["txtFirstName"];
            string lastName = Request.Form["txtLastName"];
            string street = Request.Form["txtStreet"];
            string city = Request.Form["txtCity"];
            States state = (States)Enum.Parse(typeof(States), Request.Form["ddlState"].ToString());
            string zipCode = Request.Form["txtZipCode"];
            string phoneNumber = Request.Form["txtPhoneNumber"];
            string email = Request.Form["txtEmail"];
            Client client = new Client(
                    firstName,
                    lastName,
                    new Address(street, city, state, zipCode),
                    phoneNumber,
                    email
                );
            DateTime showingTime = DateTime.Parse(Request.Form["dateShowingTime"]);
            Showing showing = new Showing(
                (int)home.HomeID,
                client,
                DateTime.Now,
                showingTime,
                ShowingStatus.Pending
                );

            if(WriteShowing.CreateNew(showing))
            {
                //return success
            } else
            {
                //make error text
            }
            return  RedirectToAction("Dashboard", "Dashboard");
        }
        //Sends user to confirmation or error page
        public IActionResult RedirectErrorConfirm(bool confirm, List<string> message)
        {
            //Send to Error Page
            TempData["Message"] = JsonConvert.SerializeObject(message);
            TempData["Action"] = "AgentDashboard";
            TempData["Controller"] = "Account";
            if (confirm)
            {
                return RedirectToAction("SharedConfirmation", "Shared");
            }
            return RedirectToAction("SharedError", "Shared");
        }
    }
}
