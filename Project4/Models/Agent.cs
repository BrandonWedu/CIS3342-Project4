namespace Project4.Models
{
	public class Agent : ICloneable<Agent>
	{
		private int agentID;
		private string agentUsername;
		private string agentPassword;
		private string agentSalt;
		private string agentFirstName;
		private string agentLastName;
		private Address agentAddress;
		private Address workAddress;
		private string agentEmail;
		private string agentPhoneNumber;
		private string agentWorkEmail;
		private string agentWorkPhoneNumber;
		private string agentCompanyName;
		private DateTime signupDate;

		public int AgentID
		{
			get { return agentID; }
			set { agentID = value; }
		}

		public string AgentUsername
		{
			get { return agentUsername; }
			set { agentUsername = value; }
		}

		public string AgentPassword
		{
			get { return agentPassword; }
			set { agentPassword = value; }
		}

		public string AgentSalt
		{
			get { return agentSalt; }
			set { agentSalt = value; }
		}

		public string AgentFirstName
		{
			get { return agentFirstName; }
			set { agentFirstName = value; }
		}

		public string AgentLastName
		{
			get { return agentLastName; }
			set { agentLastName = value; }
		}


		public Address AgentAddress
		{
			get { return agentAddress; }
			set { agentAddress = value; }
		}

		public Address WorkAddress
		{
			get { return workAddress; }
			set { workAddress = value; }
		}

		public string AgentEmail
		{ 
			get { return agentEmail; }
			set { agentEmail = value; }
		}

		public string AgentPhoneNumber
		{
			get { return agentPhoneNumber; }
			set { agentPhoneNumber = value; }
		}

		public string AgentWorkEmail
		{
			get { return agentWorkEmail; }
			set { agentWorkEmail = value; }
		}

		public string AgentWorkPhoneNumber
		{
			get { return agentWorkPhoneNumber; }
			set { agentWorkPhoneNumber = value; }
		}

		public string AgentCompanyName
		{
			get { return agentCompanyName; }
			set { agentCompanyName = value; }
		}

		public DateTime SignupDate
		{
			get { return signupDate; }
			set { signupDate = value; }
		}


		public Agent(int id, string uName, string uPass, string salt, string fName, string lName, Address agentAddress, Address workAddress, string agentEmail, string agentPhoneNumber, string workEmail, string workPhone, string companyName, DateTime singup)
		{
			AgentID = id;
			AgentUsername = uName;
			AgentPassword = uPass;
			AgentSalt = salt;
			AgentFirstName = fName;
			AgentLastName = lName;
			AgentEmail = agentEmail;
			AgentPhoneNumber = agentPhoneNumber;
			WorkAddress = workAddress;
			AgentAddress = agentAddress;
			AgentWorkEmail = workEmail;
			AgentWorkPhoneNumber = agentPhoneNumber;
			AgentCompanyName = companyName;
			SignupDate = singup;
		}

		public Agent(string uName, string uPass, string salt, string fName, string lName, Address agentAddress, Address workAddress, string agentEmail, string agentPhoneNumber, string workEmail, string workPhone, string companyName, DateTime singup)
		{
			AgentUsername = uName;
			AgentPassword = uPass;
			AgentSalt = salt;
			AgentFirstName = fName;
			AgentLastName = lName;
			AgentEmail = agentEmail;
			AgentPhoneNumber = agentPhoneNumber;
			WorkAddress = workAddress;
			AgentAddress = agentAddress;
			AgentWorkEmail = workEmail;
			AgentWorkPhoneNumber = agentPhoneNumber;
			AgentCompanyName = companyName;
			SignupDate = singup;
		}

		public Agent Clone()
		{
			return new Agent(AgentID, AgentUsername, AgentPassword, AgentSalt, AgentFirstName, AgentLastName, AgentAddress, WorkAddress, AgentEmail, AgentPhoneNumber, AgentWorkEmail, AgentWorkPhoneNumber, AgentCompanyName, SignupDate);
		}


	}
}
