namespace Project4.Models.ViewModels
{
    public class CreateAccountViewModel
    {
        public Agent agent { get; set; }

        public AgentContact contact { get; set; }

        public AgentPersonalInformation personalInformation { get; set; }

        public Company agentCompany { get; set; }


        public Address address { get; set; }

        public PasswordHasher passwordHasher { get; set; }

        public SecurityQuestionList securityQuestionList = new SecurityQuestionList();

        public AgentSecurity agentQuestionOne { get; set; }
        public AgentSecurity agentQuestionTwo { get; set; }
        public AgentSecurity agentQuestionThree { get; set; }

        // Account Info
        public string Username { get; set; }
        public string Password { get; set; }

        // Agent Personal Info
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // Agent Work Contact Info
        public string WorkStreet { get; set; }
        public string WorkCity { get; set; }
        public string WorkState { get; set; }
        public string WorkZip { get; set; }
        public string WorkPhone { get; set; }
        public string WorkEmail { get; set; }

        // Company Info

        public int Company { get; set; }


        // Security Question Info

        public string QuestionOne { get; set; }
        public string QuestionTwo { get; set; }
        public string QuestionThree { get; set; }
        public string AnswerOne { get; set; }
        public string AnswerTwo { get; set; }
        public string AnswerThree { get; set; }
    }
}
