using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Newtonsoft.Json;
using Project4.Models;
using Project4.Models.ViewModels;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Project4.Controllers
{
    //Handles Creating an account and logging in
    public class AccountController : Controller
    {
        private PasswordHasher hasher = new PasswordHasher();
        public IActionResult Login(LoginViewModel model)
        {
            // If there is a saved LoggedInAgent cookie then pass the user directly into the agent dashboard and save the agent json string to session
            if (HttpContext.Request.Cookies.ContainsKey("LoggedInAgent"))
            {
                string agentJson = HttpContext.Request.Cookies["LoggedInAgent"];
                HttpContext.Session.SetString("Agent", agentJson);
                return RedirectToAction("AgentDashboard", "AgentDashboard");
            }
            else // else there was no LoggedInAgent cookie so we pass the user to the login page
            {
                return View("Login");
            }
        }

        public IActionResult CreateAccount()
        {
            // Sends the user to the CreateAccount page
            return View("CreateAccount");
        }

        public async Task<IActionResult> FinalizeAccountCreationAsync(CreateAccountViewModel model)
        {
            bool uniqueName = true;
            // checks the user input for username to all existing agent usernames to ensure there is no duplicated username
            foreach (Agent currentAgent in ReadAgents.ReadAllAgents().List)
            {
                if (currentAgent.AgentUsername == model.Username)
                {
                    uniqueName = false;
                }
            }

            if (ModelState.IsValid == false) // if modelstate isValid eqauls false we know validation failed
            {

                ViewBag.CreateAccountError = "Please correct all the errors below and resubmit the form!"; // sets an error message
                return View("CreateAccount", model); // returns the CreateAccount page with the CreateAccountViewModel
            }

            if ( uniqueName == false) // If the username check failed and found a duplicate username then
            {
                ViewBag.CreateAccountError = "Please enter a different username! The username you are trying to enter is already taken!"; // sets error message
                return View("CreateAccount"); // returns the CreateAccount page with no view model
            }
            else
            {
                //Get selected company from the form
                int companyID = model.Company;
                Company selectedCompany = ReadCompanies.GetCompanyByCompanyID(companyID).List[0]; // Get that company information from the DB
                model.agentCompany = selectedCompany; // set the agentCompany to the selectedCompany in the viewmodel

                //Create Agent with hashedPassword
                model.passwordHasher = new PasswordHasher();
                model.passwordHasher.GenerateSalt(); // generates a unique salt to add to the user password for extra security
                string salt = model.passwordHasher.GetSalt(); // gets the generated salt
                string hashedPassword = model.passwordHasher.HashPasswordWithSalt(model.Password, salt); // hashes the userpassword and salt together
                model.agent = new Agent(model.Username, hashedPassword, salt, selectedCompany);  // sets the agent object in the model
                int agentID = WriteAgent.CreateNew(model.agent); // Saves the agent to the DB
                int actualAgentID = ReadAgents.GetAgentIDByUsername(model.Username); // Gets the newly created Agent ID from the DB
                model.agent.AgentID = actualAgentID; // sets the agent object in the model to the updated agent object with the agent id

                //Create AgentContact
                Address contactAddress = new Address(model.WorkStreet, model.WorkCity, Enum.Parse<States>(model.WorkState), model.WorkZip); // Creates a new address object with information pulled from the form
                model.contact = new AgentContact(actualAgentID, contactAddress, model.WorkPhone, model.WorkEmail); // Sets the AgentContact object in the model to a new AgentContact object with the form information
                int contactID = WriteAgentContact.CreateNew(model.contact); // writes the new AgentContact object to the database
                model.contact.AgentContactID = contactID; // sets the agentContactID to the returned value of the WriteAgentContact function in the model object


                //Create Agent PersonalInfo
                Address personalAddress = new Address(model.Street, model.City, Enum.Parse<States>(model.State), model.Zip); // Creates a new address object with information pulled from the form
				model.personalInformation = new AgentPersonalInformation(actualAgentID, model.FirstName, model.LastName, personalAddress, model.Phone, model.Email); // Sets the AgentPersonalInformation object in the model to a new AgentContact object with the form information
				int personalInfoID = WriteAgentPersonalInformation.CreateNew(model.personalInformation); // writes the new AgentPersonalInformation object to the database
				model.personalInformation.AgentInfoID = personalInfoID; // sets the AgentInfoId to the returned value of the WriteAgentContact function in the model object


				//Create security questions
				model.agentQuestionOne = new AgentSecurity(actualAgentID, model.QuestionOne, model.AnswerOne); // sets the model AgentSecurityQuesiton object to a new AgentSecurityObject with information pulled from the form
                int questionID = WriteAgentSecurityQuestion.CreateNew(model.agentQuestionOne); // Writes the new security question to the database
                model.agentQuestionOne.SecurityQuestionsID = questionID; // sets the SecurityQuestionID to the returned value of the WriteAgentSecurity function in the model object

                model.agentQuestionTwo = new AgentSecurity(actualAgentID, model.QuestionTwo, model.AnswerTwo); // sets the model AgentSecurityQuesiton object to a new AgentSecurityObject with information pulled from the form
				questionID = WriteAgentSecurityQuestion.CreateNew(model.agentQuestionTwo); // Writes the new security question to the database
				model.agentQuestionTwo.SecurityQuestionsID = questionID; // sets the SecurityQuestionID to the returned value of the WriteAgentSecurity function in the model object

				model.agentQuestionThree = new AgentSecurity(actualAgentID, model.QuestionThree, model.AnswerThree);// sets the model AgentSecurityQuesiton object to a new AgentSecurityObject with information pulled from the form
				questionID = WriteAgentSecurityQuestion.CreateNew(model.agentQuestionThree); // Writes the new security question to the database
				model.agentQuestionThree.SecurityQuestionsID = questionID; // sets the SecurityQuestionID to the returned value of the WriteAgentSecurity function in the model object

				//Send User an email with a like and code
				Random random = new Random();
                int code = random.Next(1000, 10000);
                //set code in db
                WriteVerification.CreateNew(agentID, code);

                EmailInfo info = new EmailInfo(
                        model.WorkEmail, 
                        "tui78495@temple.edu",
                        "Account Verification",
                        $"Please click this link or enter the code on the website to verify your account\n" +
                        $"Code: {code}\n" +
                        $"<a>https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/Project4/VerifyAccountWithLink/{agentID}/{code}</a>"
                    );

                //Call the Email API and send the email
                StringContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(info), Encoding.UTF8, "application/json");
                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await httpClient.PostAsync("https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPITest/Email/SendToTempleEmail", content);
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Response Body: {responseBody}");
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                TempData["AgentID"] = agentID;
                return View("VerifyAccount");
            }
        }

        public IActionResult VerifyAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyAccountWithCode(int code) 
        {
            if (ReadVerification.GetVerifiedCode((int)TempData["AgentID"]) == code)
            {
                //Set agent verified to true
                WriteVerification.Verify((int)TempData["AgentID"]);
                return View("Login"); 
            }
            TempData["AgentID"] = TempData["AgentID"];
            TempData["Error"] = "Code Invalid";
            return View("VerifyAccount");
        }

        [HttpGet("VerifyAccountWithLink/{agentID}/{code}")]
        public IActionResult VerifyAccountWithLink(int agentID, int code)
        {
            //Check if code and AgentID are valid
            if (ReadVerification.GetVerifiedCode(agentID) == code)
            {
                //Set agent verified to true
                WriteVerification.Verify((int)TempData["AgentID"]);
                return View("Login"); 
            }
            TempData["Error"] = "Code Invalid";
            return View("VerifyAccount");
        }

        public IActionResult CreateCompany()
        {
            // Returns the CreateCompany page
            return View();
        }
        public IActionResult FinalizeCompanyCreation(CreateCompanyViewModel model)
        {
			if (ModelState.IsValid == false) // if the model IsValid state is false then validation failed
			{
				ViewBag.CreateCompanyError = "Please correct all the errors below and resubmit the form!"; // sets an error message
				return View("CreateCompany", model); // returns the CreateCompany page with the CreateCompanyViewModel
			}
            else // else validation passed
            {
				//Create Company
				Address newAddress = new Address(model.CompanyStreet, model.CompanyCity, Enum.Parse<States>(model.CompanyState), model.CompanyZip); // sets a new address object with information from the form
				Company newCompany = new Company(model.CompanyName, newAddress, model.CompanyPhone, model.CompanyEmail); // sets a new company object with information from the form
				int companyID = WriteCompany.CreateNew(newCompany); // creates the new company in the database
				int actualCompanyID = ReadCompanies.GetComapnyByNameAndAddress(model.CompanyName, newAddress).List[0].CompanyID; // gets the companyId of the newly created company from the DB
				return View("CreateAccount");
			}

        }
        [HttpPost]
        public IActionResult ForgotPassword(string username)
        {
            Agent currentAgent = ReadAgents.GetAgentByUsername(username);
            if (currentAgent != null) // if agent is not null then
            {
                string agentJson = JsonConvert.SerializeObject(currentAgent);
                HttpContext.Session.SetString("RecoveryAgent", agentJson); // write the agent json string to session

                Random randomNumber = new Random();
                int randomInt = randomNumber.Next(0, ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(currentAgent.AgentID).List.Count - 1); // picks a random number out of the count of security questions

                AgentSecurity randomQuestion = ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(currentAgent.AgentID).List[randomInt]; // picks the random security question based on the generated int
                string questionJson = JsonConvert.SerializeObject(randomQuestion);
                HttpContext.Session.SetString("RecoveryQuestion", questionJson); // writes the question json to session

				ViewBag.Question = randomQuestion.Question.ToString(); // writes the question to the viewbag
                return View("ForgotPasswordSecurity"); // returns the ForgotPasswordSecurity Page
            }
            else // the agent is not valid agent object
            {
                ViewBag.ForgotPasswordError = "No account found with that username! Please try again!"; // Set an error message
                return View(); // returns the ForgotPassword page
            }
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            // returns the ForgotPassword page
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPasswordSecurity()
        {
            // returns ForgotPasswordSecurity page
            return View("ForgotPasswordSecurity");
        }

        [HttpPost]
        public IActionResult ForgotPasswordSecurity(string answer)
        {
			string quesitonJson = HttpContext.Session.GetString("RecoveryQuestion");
			AgentSecurity question = JsonConvert.DeserializeObject<AgentSecurity>(quesitonJson);
            if (question.Answer.ToString() == answer.ToString()) // if the user answer equals the stored answer then
            {
                // returns ResetPassword page
                return View("ResetPassword");
            }
            else // answers didnt match
            {
                ViewBag.Question = question.Question.ToString(); // resets the question to viewbag
                ViewBag.ForgotPasswordError = "Incorrect Answer To Security Question! Please Try Again!"; // sets an error message
				return View(); // returns ForgotPasswordSecurity page
            }

        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            // returns ResetPassword page
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string newPassword, string newPasswordVerify)
        {

            if (newPassword == newPasswordVerify) // if the passwords match each other then
            {
                if (newPassword.Length > 6) // if the minimum lenght is atleast 6
                {
					string agentJson = HttpContext.Session.GetString("RecoveryAgent");
					Agent currentAgent = JsonConvert.DeserializeObject<Agent>(agentJson); // deserializes the agent and sets it to a Agent object
					PasswordHasher hasher = new PasswordHasher();
					hasher.GenerateSalt(); // makes a new random salt
					string salt = hasher.GetSalt(); // gets the generated salt
					string hashedPassword = hasher.HashPasswordWithSalt(newPassword, salt); // hashes the password with the generated salt
					WriteAgent.UpdateAgentPassword(currentAgent, hashedPassword, salt); //updates the users password in the database


                    // Clean up session objects
                    HttpContext.Session.Remove("RecoveryAgent");
                    HttpContext.Session.Remove("RecoveryQuestion");
                    // returns the Login page
					return View("Login");
				}
                else
                {
                    ViewBag.ResetPasswordError = "Your New Password Must Be Longer Than 6 Characters!"; // sets an error message
                    return View(); // returns the ResetPassword page
                }
            }
            else
            {
                ViewBag.ResetPasswordError = "Passwords Did Not Match! Please Re-enter Your New Passwords!"; // sets an error message
                return View(); // returns the ResetPassword page
            }

        }

        public IActionResult TryLogin(LoginViewModel model)
        {
            Agent agent = ReadAgents.GetAgentByUsername(model.Username);

            if (agent != null && agent.AgentVerified == true) // if the current agent is not null and the account is verified then
            {
                string salt = agent.AgentPasswordSalt; // gets the agents salt
                string hashedPW = agent.AgentPassword; // gets the hashed agent password
                string userPasswordSalted = model.Password + salt; // gets the entered password and combines the salt to id
                //string ReHashedPW = hasher.HashPasswordWithSalt(model.Password, salt); // this was used for debugging
                if (hasher.VerifyPassword(agent.AgentUsername, model.Password) == true) // if the stored hash is equal to the new hash that was made with the salt and entered password then
                {
                    if (model.SaveCookie.Equals("true", StringComparison.OrdinalIgnoreCase)) // if save cookie checkbox is checked
                    {
                        string agentJSON = System.Text.Json.JsonSerializer.Serialize(agent); // gets the agent objects json string
                        CookieOptions agentCookieOptions = new CookieOptions(); // makes a new cookie
                        agentCookieOptions.HttpOnly = true;
                        agentCookieOptions.Secure = true;
                        agentCookieOptions.Expires = DateTime.Now.AddDays(1);
                        HttpContext.Response.Cookies.Append("LoggedInAgent", agentJSON, agentCookieOptions); // adds the cookie called LoggedInAgent with the agent json string as it value
                    }

                    //Add Agent to Session
                    string agentJSONSession = System.Text.Json.JsonSerializer.Serialize(agent);
                    HttpContext.Session.SetString("Agent", agentJSONSession);

                    //Agent loggedInAccount = ReadAgents.GetAgentByAgentID(agent.AgentID);
                    return RedirectToAction("AgentDashboard", "AgentDashboard"); // sends to user to the AgentDashbord page of the AgentDashboard controller
                }
                else
                {
                    ViewBag.LoginError = "Incorrect Password! Please try again!"; // sets an error
                }
            }
            else
            {
                ViewBag.LoginError = "Please enter a proper username or make sure your account is verified!"; // sets an error
            }

            return View("Login"); // login failed so reload login page

        }

    }
}
