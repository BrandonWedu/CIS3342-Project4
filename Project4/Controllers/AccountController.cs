using Microsoft.AspNetCore.Mvc;
using Project4.Models;
using System.Security.Cryptography;

namespace Project4.Controllers
{
    //Handles Creating an account and logging in
    public class AccountController : Controller
    {
        private PasswordHasher hasher = new PasswordHasher();
        // Going to need to make some ViewModels to include all the information I need
        public IActionResult Login()
        {
            return View("Login");
        }

        public IActionResult CreateAccount(string username, string password, string fName, string lName)
        {
            //Get all agent & company information
            //Get secuirty questions answers
            return View("Login");
        }

        public IActionResult ForgotPassword(string username, string answer)
        {
            Random randomGenerator = new Random();
            Agent agent = ReadAgents.GetAgentByUsername(username).List[0];
            AgentSecuritys securityQuestions = agent.AgentSecuirtyQuestions;
            int questions = securityQuestions.List.Count;
            int randomQuestion = randomGenerator.Next(questions);

            return View("ForgotPassword", securityQuestions.List[randomQuestion]);
        }

        public IActionResult TryLogin(string username, string password)
        {
            Agent agent = ReadAgents.GetAgentByUsername(username).List[0];

            if (agent != null)
            {
                string salt = agent.AgentPasswordSalt;
                string hashedPW = agent.AgentPassword;
                string userPasswordSalted = password + salt;

                if (hasher.VerifyPassword(hashedPW, userPasswordSalted))
                {
                    Agent loggedInAccount = ReadAgents.GetAgentByAgentID(agent.AgentID).List[0];
                    // need to write loggedInAccount to session
                    return View("Confirmation", loggedInAccount); // confirmation just sits for a few seconds then redirects to agent dashboard
                }
            }

            return View("Login"); // login falied so reload login page
        }

        public IActionResult TryForgotPassword(AgentSecurity question, string answer, string newPassword)
        {
            if (question.Answer == answer) // Answer to security question was correct so update password and load the login page
            {
                WriteAgent.UpdatePassword(agent, newPassword); 
                return View("Login");
            }
            else // secuirty question was not correct so reload forgotPassword page or login apge
            {
                return View("ForgotPassword");
            }
        }
    }
}
