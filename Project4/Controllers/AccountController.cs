using Microsoft.AspNetCore.Mvc;
using Project4.Models;
using Project4.Models.ViewModels;
using System.Security.Cryptography;

namespace Project4.Controllers
{
    //Handles Creating an account and logging in
    public class AccountController : Controller
    {
        private PasswordHasher hasher = new PasswordHasher();
        // Going to need to make some ViewModels to include all the information I need
        public IActionResult Login(LoginViewModel model)
        {
            return View("Login");
        }

        public IActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        public IActionResult FinalizeAccountCreation(CreateAccountViewModel model)
        {
            if (model.Username == "")
            {
                return View("CreateAccount");
            }
            else
            {
                // Need to add username check
                //Create Company
                Address newAddress = new Address(model.CompanyStreet, model.CompanyCity, Enum.Parse<States>(model.CompanyState), model.CompanyZip);
                model.company = new Company(model.CompanyName, newAddress, model.CompanyPhone, model.CompanyEmail);
                int companyID = WriteCompany.CreateNew(model.company);
                int actualCompanyID = ReadCompanies.GetComapnyByNameAndAddress(model.CompanyName, newAddress).List[0].CompanyID;
                model.company.CompanyID = actualCompanyID;

                //Create Agent with hashedPassword
                model.passwordHasher = new PasswordHasher();
                model.passwordHasher.GenerateSalt();
                string salt = model.passwordHasher.GetSalt();
                string hashedPassword = model.passwordHasher.HashPasswordWithSalt(model.Password, salt);
                model.agent = new Agent(model.Username, hashedPassword, salt, model.company);
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
                Address personalAddress = new Address(model.Street, model.City, model.State, model.Zip);
                model.personalInformation = new AgentPersonalInformation(actualAgentID, model.FirstName, model.LastName, personalAddress, model.Phone, model.Email);
                int personalInfoID = WriteAgentPersonalInformation.CreateNew(model.personalInformation);
                model.personalInformation.AgentInfoID = personalInfoID;


                //Create security questions
                model.agentQuestionOne = new AgentSecurity(actualAgentID, SecurityQuestionList.GetAllQuestions()[0].ToString(), model.AnswerOne);
                int questionID = WriteAgentSecurityQuestion.CreateNew(model.agentQuestionOne);
                model.agentQuestionOne.SecurityQuestionsID = questionID;

                model.agentQuestionTwo = new AgentSecurity(actualAgentID, SecurityQuestionList.GetAllQuestions()[1].ToString(), model.AnswerTwo);
                questionID = WriteAgentSecurityQuestion.CreateNew(model.agentQuestionTwo);
                model.agentQuestionTwo.SecurityQuestionsID = questionID;

                model.agentQuestionThree = new AgentSecurity(actualAgentID, SecurityQuestionList.GetAllQuestions()[2].ToString(), model.AnswerThree);
                questionID = WriteAgentSecurityQuestion.CreateNew(model.agentQuestionThree);
                model.agentQuestionThree.SecurityQuestionsID = questionID;

                return View("Login");
            }
        }

        public IActionResult ForgotPassword(string username, string answer)
        {
            Random randomGenerator = new Random();
            Agent agent = ReadAgents.GetAgentByUsername(username);
            AgentSecuritys securityQuestions = agent.AgentSecuirtyQuestions;
            int questions = securityQuestions.List.Count;
            int randomQuestion = randomGenerator.Next(questions);

            return View("ForgotPassword", securityQuestions.List[randomQuestion]);
        }

        public IActionResult TryLogin(LoginViewModel model)
        {
            Agent agent = ReadAgents.GetAgentByUsername(model.Username);

            if (agent != null)
            {
                string salt = agent.AgentPasswordSalt;
                string hashedPW = agent.AgentPassword;
                string userPasswordSalted = model.Password + salt;

                if (hasher.VerifyPassword(hashedPW, userPasswordSalted))
                {
                    Agent loggedInAccount = ReadAgents.GetAgentByAgentID(agent.AgentID);
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
