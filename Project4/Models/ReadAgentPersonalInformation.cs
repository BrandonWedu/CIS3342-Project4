using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class ReadAgentPersonalInformation
	{
		private DBConnect databaseHandler = new DBConnect();
		private AgentPersonalInformationList allAgentPersonalInfo = new AgentPersonalInformationList();

		public ReadAgentPersonalInformation()
		{
			allAgentPersonalInfo = new AgentPersonalInformationList();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "SelectAllAgentPersonalInformation";
			DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentContactData.Rows)
			{
				allAgentPersonalInfo.Add(new AgentPersonalInformation((int)row["AgentInfoID"], (int)row["AgentID"], row["FirstName"].ToString(), row["LastName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["HomeAddress"]), row["PhoneNumber"].ToString(), row["Email"].ToString()));
			}
		}

		public AgentPersonalInformationList GetAllAgentPerosnalInformation()
		{
			return allAgentPersonalInfo;
		}

		public AgentPersonalInformationList GetAgentPeronsalInformationByAgentID(int id)
		{
			AgentPersonalInformationList selectedAgent = new AgentPersonalInformationList();
			foreach (AgentPersonalInformation currentInfo in allAgentPersonalInfo.List)
			{
				if (currentInfo.AgentID == id)
				{
					selectedAgent.Add(currentInfo);
				}
			}

			return selectedAgent;
		}
	}
}
