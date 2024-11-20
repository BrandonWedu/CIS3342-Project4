namespace Project4.Models
{
	public class AgentContact : ICloneable<AgentContact>
	{
		private int agentContactID;
		private int agentID;
		private Address workAddress;
		private string phoneNumber;
		private string email;

		public int AgentContactID
		{
			get { return agentContactID; }
			set { agentContactID = value; }
		}

		public int AgentID
		{
			get { return agentID; }
			set { agentID = value; }
		}

		public Address WorkAddress
		{
			get { return workAddress; }
			set { workAddress = value; }
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


		public AgentContact(int id, int agentID, Address workAddress, string phone, string email) 
		{
			AgentContactID = id;
			AgentID = agentID;
			WorkAddress = workAddress;
			PhoneNumber = phone;
			Email = email;
		}


		public AgentContact Clone()
		{
			return new AgentContact(AgentContactID, AgentID, WorkAddress, PhoneNumber, Email);
		}
	}
}
