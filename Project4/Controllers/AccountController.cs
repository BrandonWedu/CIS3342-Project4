using Microsoft.AspNetCore.Mvc;
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
            return View("Login");
        }

        public IActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        public async Task<IActionResult> FinalizeAccountCreationAsync(CreateAccountViewModel model)
        {
            bool uniqueName = true;
            foreach (Agent currentAgent in ReadAgents.ReadAllAgents().List)
            {
                if (currentAgent.AgentUsername == model.Username)
                {
                    uniqueName = false;
                }
            }

            if (model.Username == "" || uniqueName == false)
            {
                return View("CreateAccount");
            }
            else
            {
                // Need to add username check
                //Get selected company
                int companyID = model.Company;
                Company selectedCompany = ReadCompanies.GetCompanyByCompanyID(companyID).List[0];
                model.agentCompany = selectedCompany;

                //Create Agent with hashedPassword
                model.passwordHasher = new PasswordHasher();
                model.passwordHasher.GenerateSalt();
                string salt = model.passwordHasher.GetSalt();
                string hashedPassword = model.passwordHasher.HashPasswordWithSalt(model.Password, salt);
                model.agent = new Agent(model.Username, hashedPassword, salt, selectedCompany);
                int agentID = WriteAgent.CreateNew(model.agent);
                int actualAgentID = ReadAgents.GetAgentIDByUsername(model.Username);
                model.agent.AgentID = actualAgentID;

                //Create AgentContact
                Console.WriteLine(model.State);
                Console.WriteLine(model.WorkState);
                string test = model.State;
                Address contactAddress = new Address(model.WorkStreet, model.WorkCity, Enum.Parse<States>(model.WorkState), model.WorkZip);
                model.contact = new AgentContact(actualAgentID, contactAddress, model.WorkPhone, model.WorkEmail);
                int contactID = WriteAgentContact.CreateNew(model.contact);
                model.contact.AgentContactID = contactID;


                //Create Agent PersonalInfo
                // Make sure address gets included in this object class
                Address personalAddress = new Address(model.Street, model.City, Enum.Parse<States>(model.State), model.Zip);
                model.personalInformation = new AgentPersonalInformation(actualAgentID, model.FirstName, model.LastName, personalAddress, model.Phone, model.Email);
                int personalInfoID = WriteAgentPersonalInformation.CreateNew(model.personalInformation);
                model.personalInformation.AgentInfoID = personalInfoID;


                //Create security questions
                model.agentQuestionOne = new AgentSecurity(actualAgentID, model.QuestionOne, model.AnswerOne);
                int questionID = WriteAgentSecurityQuestion.CreateNew(model.agentQuestionOne);
                model.agentQuestionOne.SecurityQuestionsID = questionID;

                model.agentQuestionTwo = new AgentSecurity(actualAgentID, model.QuestionTwo, model.AnswerTwo);
                questionID = WriteAgentSecurityQuestion.CreateNew(model.agentQuestionTwo);
                model.agentQuestionTwo.SecurityQuestionsID = questionID;

                model.agentQuestionThree = new AgentSecurity(actualAgentID, model.QuestionThree, model.AnswerThree);
                questionID = WriteAgentSecurityQuestion.CreateNew(model.agentQuestionThree);
                model.agentQuestionThree.SecurityQuestionsID = questionID;

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
                StringContent content = new StringContent(JsonSerializer.Serialize(info), Encoding.UTF8, "application/json");
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
            return View();
        }
        public IActionResult FinalizeCompanyCreation(CreateCompanyViewModel model)
        {
            //Create Company
            Address newAddress = new Address(model.CompanyStreet, model.CompanyCity, Enum.Parse<States>(model.CompanyState), model.CompanyZip);
            Company newCompany = new Company(model.CompanyName, newAddress, model.CompanyPhone, model.CompanyEmail);
            int companyID = WriteCompany.CreateNew(newCompany);
            int actualCompanyID = ReadCompanies.GetComapnyByNameAndAddress(model.CompanyName, newAddress).List[0].CompanyID;
            return View("CreateAccount");
        }
        [HttpPost]
        public IActionResult ForgotPassword(string username)
        {
            Agent currentAgent = ReadAgents.GetAgentByUsername(username);
            if (currentAgent != null)
            {
                TempData["Username"] = username;
                Random randomNumber = new Random();
                int randomInt = randomNumber.Next(0, ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(currentAgent.AgentID).List.Count - 1);
                ViewBag.Question = ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(currentAgent.AgentID).List[randomInt].Question.ToString();
                TempData["QuestionInt"] = randomInt;
                return View("ForgotPasswordSecurity");
            }
            else
            {
                //Make error message
                return View();
            }
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPasswordSecurity()
        {
            string username = TempData["Username"].ToString();
            Agent currentAgent = ReadAgents.GetAgentByUsername(username);
            if (currentAgent == null)
            {
                return View("ForgotPassword");
            }
            else
            {
                //AgentSecuritys agentSecurityQuestions = ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(currentAgent.AgentID);
                //Random randomNumber = new Random();
                //int randomInt = randomNumber.Next(0, agentSecurityQuestions.List.Count - 1);
                //Console.WriteLine(randomInt);
                return View("ForgotPasswordSecurity");
            }

        }

        [HttpPost]
        public IActionResult ForgotPasswordSecurity(string answer)
        {
            string username = TempData["Username"].ToString();
            int questionInt = (int)TempData["QuestionInt"];
            Agent currentAgent = ReadAgents.GetAgentByUsername(username);
            AgentSecuritys agentSecurityQuestions = ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(currentAgent.AgentID);
            AgentSecurity currentQuestion = ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(currentAgent.AgentID).List[questionInt];
            if (currentQuestion.Answer.ToString() == answer.ToString())
            {
                TempData["Username"] = username;
                return View("ResetPassword");
            }
            else
            {

                //Make error message
                return View();
            }

        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            string username = TempData["Username"].ToString();

            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string newPassword, string newPasswordVerify)
        {
            if (newPassword == newPasswordVerify)
            {
                Console.WriteLine("Passwords Matched");
                string username = TempData["Username"].ToString();
                Agent currentAgent = ReadAgents.GetAgentByUsername(username);
                PasswordHasher hasher = new PasswordHasher();
                hasher.GenerateSalt();
                string salt = hasher.GetSalt();
                string hashedPassword = hasher.HashPasswordWithSalt(newPassword, salt);
                WriteAgent.UpdateAgentPassword(currentAgent, hashedPassword, salt);
                return View("Login");
            }
            else
            {
                Console.WriteLine("Passwords did not Match");
                return View();
            }

        }

        public IActionResult TryLogin(LoginViewModel model)
        {
            Agent agent = ReadAgents.GetAgentByUsername(model.Username);

            if (agent != null && agent.AgentVerified == true)
            {
                Console.WriteLine("Agent found!");
                string salt = agent.AgentPasswordSalt;
                Console.WriteLine("Salt: " + salt);
                string hashedPW = agent.AgentPassword;
                Console.WriteLine("HashedPW: " + hashedPW);
                string userPasswordSalted = model.Password + salt;
                Console.WriteLine("SaltedPW: " + userPasswordSalted);


                string ReHashedPW = hasher.HashPasswordWithSalt(model.Password, salt);
                Console.WriteLine("ReHashedPW: " + ReHashedPW);
                if (hasher.VerifyPassword(agent.AgentUsername, model.Password) == true)
                {
                    if (model.SaveCookie.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        string agentJSON = System.Text.Json.JsonSerializer.Serialize(agent);
                        CookieOptions agentCookieOptions = new CookieOptions();
                        agentCookieOptions.HttpOnly = true;
                        agentCookieOptions.Secure = true;
                        agentCookieOptions.Expires = DateTime.Now.AddDays(1);
                        HttpContext.Response.Cookies.Append("LoggedInAgent", agentJSON, agentCookieOptions);
                    }
                    Console.WriteLine("Password verified!");

                    //Add Agent to Session
                    string agentJSONSession = System.Text.Json.JsonSerializer.Serialize(agent);
                    HttpContext.Session.SetString("Agent", agentJSONSession);

                    Agent loggedInAccount = ReadAgents.GetAgentByAgentID(agent.AgentID);
                    return RedirectToAction("AgentDashboard", "AgentDashboard"); // confirmation just sits for a few seconds then redirects to agent dashboard
                }
                else
                {
                    Console.WriteLine("Password verification failed!");
                }
            }
            else
            {
                Console.WriteLine("No agent found with the given username or account is not verified!");
            }

            return View("Login"); // login failed so reload login page

        }

        public IActionResult TryForgotPassword(AgentSecurity question, string answer, string newPassword)
        {
            if (question.Answer == answer) // Answer to security question was correct so update password and load the login page
            {
                //WriteAgent.UpdatePassword(agent, newPassword); 
                return View("Login");
            }
            else // secuirty question was not correct so reload forgotPassword page or login apge
            {
                return View("ForgotPassword");
            }
        }
    }
}
