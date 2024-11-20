using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class ReadAgents
	{
		private DBConnect databaseHandler = new DBConnect();
		private AgentList allAgents = new AgentList();
		private ReadAgentContact allAgentContact = new ReadAgentContact();
		private ReadCompanies allCompanies = new ReadCompanies();
		private ReadAgentPersonalInformation allAgentInfo = new ReadAgentPersonalInformation();
		private ReadAgentSecurity allAgentSecurity = new ReadAgentSecurity();


		public ReadAgents()
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "SelectAllAgents";
			DataTable agentData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentData.Rows)
			{
				int agentID = (int)row["AgentID"];
				int companyID = (int)row["CompanyID"];
				allAgents.Add(new Agent((int)row["AgentID"], row["AgentUsername"].ToString(), row["AgentPassword"].ToString(), row["AgentPasswordSalt"].ToString(), allCompanies.GetCompanyByCompanyID(companyID).List[0], allAgentContact.GetAgentContactByAgentID(agentID).List[0], allAgentInfo.GetAgentPeronsalInformationByAgentID(agentID).List[0], allAgentSecurity.GetAgentSecurityQuestionsByAgentID(agentID)));
			}
		}

		public AgentList GetAllAgents()
		{
			return allAgents;
		}

		public AgentList GetAgentByAgentID(int id)
		{
			AgentList selectedAgent = new AgentList();
			foreach(Agent agent in allAgents.List)
			{
				if (agent.AgentID == id)
				{
					selectedAgent.Add(agent);
				}
			}
			return selectedAgent;
		}


	}
}
