namespace Project4.Models
{
	public class AgentPersonalInformation : ICloneable<AgentPersonalInformation>
	{
		private int agentInfoID;
		private int agentID;
		private string firstName;
		private string lastName;
		private Address homeAddress;
		private string phoneNumber;
		private string email;

		public int AgentInfoID
		{
			get { return agentInfoID; }
			set { agentInfoID = value; }
		}

		public int AgentID
		{
			get { return agentID; }
			set { agentID = value; }
		}

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		public Address HomeAddress
		{
			get { return homeAddress; }
			set { homeAddress = value; }
		}

		public string PhoneNumber
		{
			get { return phoneNumber; }
			set { phoneNumber = value; }
		}

		public string Email
		{
			get { return email; }
			set { email = value; }
		}

		public AgentPersonalInformation(int id, int agentId, string fName, string lName, Address homeAddress, string phone, string email)
		{
			AgentInfoID = id;
			AgentID = agentId;
			FirstName = fName;
			LastName = lName;
			PhoneNumber = phone;
			Email = email;
		}

		public AgentPersonalInformation Clone()
		{
			return new AgentPersonalInformation(AgentInfoID, AgentID, FirstName, LastName, HomeAddress, PhoneNumber, Email);
		}
	}
}
