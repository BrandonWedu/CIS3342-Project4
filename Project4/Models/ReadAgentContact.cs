using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class ReadAgentContact
	{
		private DBConnect databaseHandler = new DBConnect();
		private AgentContactList allAgentContact = new AgentContactList();

		public ReadAgentContact()
		{
			allAgentContact = new AgentContactList();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "SelectAllAgentContact";
			DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentContactData.Rows)
			{
				allAgentContact.Add(new AgentContact((int)row["AgentContactID"], (int)row["AgentID"], Serializer.DeserializeData<Address>((byte[])row["WorkAddress"]), row["PhoneNumber"].ToString(), row["Email"].ToString()));
			}

		}

		public AgentContactList GetAllAgentContact()
		{
			return allAgentContact;
		}

		public AgentContactList GetAgentContactByAgentID(int agentID)
		{
			AgentContactList agentContact = new AgentContactList();

			foreach (AgentContact contactInfo in allAgentContact.List)
			{
				if (contactInfo.AgentID == agentID)
				{
					agentContact.Add(contactInfo);
				}
			}
			return agentContact;
		}
	}
}
