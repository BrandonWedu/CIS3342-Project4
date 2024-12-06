using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project4.Models;
using Project4.Models.ViewModels;
using System.Text.Json;
using System.Text.RegularExpressions;

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
                try
                {
                    Showings showings = ReadShowing.GetShowingsByAgentID(agent.AgentID);
                    TempData["Showings"] = showings;
                    return View();
                } catch (Exception ex)
                {
                    //Send to Error Page
                    return ViewShowingErrorConfirm(false, new List<string>
                    {
                        $"Error Message: {ex}"
                    });
                }
            }
            //Send to Error Page
            return ViewShowingErrorConfirm(false, new List<string>
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
                    return ViewShowingErrorConfirm(false, new List<string>
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
                    return ViewShowingErrorConfirm(false, new List<string>
                    {
                        $"ShowingID: {showingID} is already Status: {showingStatus}"
                    });
                }
                else if(WriteShowing.UpdateStatus(showingID, showingStatus))
                {
                    return ViewShowingErrorConfirm(true, new List<string>
                    {
                        $"ShowingID: {showingID} status updated to Status: {showingStatus.ToString()}"
                    });
                }
                else
                {
                    return ViewShowingErrorConfirm(false, new List<string>
                    {
                        $"{showingID} status NOT updated"
                    });
                }
            }
            catch (Exception ex)
            {
                //Send to Error Page
                return ViewShowingErrorConfirm(false, new List<string>
                {
                    $"Error Message: {ex}"
                });
            }
        }

        public IActionResult ScheduleShowing()
        {
            TempData.Clear();
            try
            {
                string homeJson = HttpContext.Session.GetString("ShowingHome");
                Home showingHome = JsonConvert.DeserializeObject<Home>(homeJson);
                TempData["Home"] = showingHome;
                return View();

            } catch (Exception ex)
            {
                //Send to Error Page
                return MakeShowingErrorConfirm(false, new List<string>
                {
                    $"Error Message: {ex}"
                });
            }
        }

        [HttpPost]
        public IActionResult ShowingRequest()
        {
            if (!ValidateMakeShowing()) 
            {
                Home showingHome = JsonConvert.DeserializeObject<Home>(HttpContext.Session.GetString("ShowingHome"));
                TempData["Home"] = showingHome;
                return View("ScheduleShowing"); 
            }
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

            try
            {
                if(WriteShowing.CreateNew(showing))
                {
                    return MakeShowingErrorConfirm(true, new List<string>
                    {
                        $"Congratulations {showing.Client.FirstName} {showing.Client.LastName}!",
                        $"You made a showing request for {showing.ShowingTime}"
                    });
                }
                else
                {
                    return MakeShowingErrorConfirm(false, new List<string>
                    {
                        $"Sorry {showing.Client.FirstName} {showing.Client.LastName}.",
                        $"failed to make a showing request"
                    });
                }
            }
            catch (Exception ex)
            {
                return MakeShowingErrorConfirm(false, new List<string>
                {
                    $"Error Message: {ex}"
                });
            }
        }
        //Validate Make Showing inputs
        public bool ValidateMakeShowing()
        {
            RetainData();
            List<string> errors = new List<string>();
            // Validate Fields
            if (string.IsNullOrEmpty(Request.Form["txtFirstName"]) || !Regex.IsMatch(Request.Form["txtFirstName"], @"\S+"))
            {
                errors.Add("First Name is invalid or cannot be null.");
            }
            if (string.IsNullOrEmpty(Request.Form["txtLastName"]) || !Regex.IsMatch(Request.Form["txtLastName"], @"\S+"))
            {
                errors.Add("Last Name is invalid or cannot be null.");
            }
            if (string.IsNullOrEmpty(Request.Form["txtStreet"]) || !Regex.IsMatch(Request.Form["txtStreet"], @"\S+"))
            {
                errors.Add("Street is invalid or cannot be null.");
            }
            if (string.IsNullOrEmpty(Request.Form["ddlState"]) || !Regex.IsMatch(Request.Form["ddlState"], @"\S+"))
            {
                errors.Add("State is invalid or cannot be null.");
            }
            if (string.IsNullOrEmpty(Request.Form["txtZipCode"]) || !Regex.IsMatch(Request.Form["txtZipCode"], @"^\d{5}(-\d{4})?$"))
            {
                errors.Add("Zip Code is invalid. It must be 5 digits or in ZIP+4 format.");
            }
            if (string.IsNullOrEmpty(Request.Form["txtPhoneNumber"]) || !Regex.IsMatch(Request.Form["txtPhoneNumber"], @"^\d{9,10}$"))
            {
                errors.Add("Phone Number is invalid. It must be 9 or 10 digits.");
            }
            if (string.IsNullOrEmpty(Request.Form["txtEmail"]) || !Regex.IsMatch(Request.Form["txtEmail"], @"^[^@\s]+@temple\.edu$"))
            {
                errors.Add("Email is invalid. It must be a Temple University email address.");
            }
            if (string.IsNullOrEmpty(Request.Form["dateShowingTime"]) || !DateTime.TryParse(Request.Form["dateShowingTime"], out DateTime showingTime) || showingTime < DateTime.Now)
            {
                errors.Add("Showing Time is invalid, cannot be null, and must be a future date.");
            }


            if (errors.Count > 0 )
            {
                TempData["Errors"] = errors;
                return false;
            }
            return true;
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

        //Sends user to confirmation or error page
        public IActionResult ViewShowingErrorConfirm(bool confirm, List<string> message)
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
        public IActionResult MakeShowingErrorConfirm(bool confirm, List<string> message)
        {
            //Send to Error Page
            TempData["Message"] = JsonConvert.SerializeObject(message);
            TempData["Action"] = "Dashboard";
            TempData["Controller"] = "Dashboard";
            if (confirm)
            {
                return RedirectToAction("SharedConfirmation", "Shared");
            }
            return RedirectToAction("SharedError", "Shared");
        }
    }
}
