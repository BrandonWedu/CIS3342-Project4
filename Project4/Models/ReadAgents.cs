using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class ReadAgents
	{
		internal static Agents ReadAllAgents()
		{
            DBConnect databaseHandler = new DBConnect();
            Agents allAgents = new Agents();
            SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "SelectAllAgents";
			DataTable agentData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentData.Rows)
			{
				int agentID = (int)row["AgentID"];
				int companyID = (int)row["CompanyID"];
				allAgents.Add(new Agent((int)row["AgentID"], row["AgentUsername"].ToString(), row["AgentPassword"].ToString(), row["AgentPasswordSalt"].ToString(), ReadCompanies.GetCompanyByCompanyID(companyID).List[0], ReadAgentContact.GetAgentContactByAgentID(agentID).List[0], ReadAgentPersonalInformation.GetAgentPeronsalInformationByAgentID(agentID).List[0], ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(agentID)));
			}

			return allAgents;
		}


		internal static Agents GetAgentByAgentID(int id)
		{
            DBConnect databaseHandler = new DBConnect();
            Agents allAgents = new Agents();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectAllAgents";
            DataTable agentData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentData.Rows)
            {
                int agentID = (int)row["AgentID"];
                int companyID = (int)row["CompanyID"];
                allAgents.Add(new Agent((int)row["AgentID"], row["AgentUsername"].ToString(), row["AgentPassword"].ToString(), row["AgentPasswordSalt"].ToString(), ReadCompanies.GetCompanyByCompanyID(companyID).List[0], ReadAgentContact.GetAgentContactByAgentID(agentID).List[0], ReadAgentPersonalInformation.GetAgentPeronsalInformationByAgentID(agentID).List[0], ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(agentID)));
            }
            Agents selectedAgent = new Agents();
			foreach(Agent agent in allAgents.List)
			{
				if (agent.AgentID == id)
				{
					selectedAgent.Add(agent);
				}
			}
			return selectedAgent;
		}

		internal static Agents GetAgentByUsername(string userName)
		{
            DBConnect databaseHandler = new DBConnect();
            Agents allAgents = new Agents();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectAllAgents";
            DataTable agentData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentData.Rows)
            {
                int agentID = (int)row["AgentID"];
                int companyID = (int)row["CompanyID"];
                allAgents.Add(new Agent((int)row["AgentID"], row["AgentUsername"].ToString(), row["AgentPassword"].ToString(), row["AgentPasswordSalt"].ToString(), ReadCompanies.GetCompanyByCompanyID(companyID).List[0], ReadAgentContact.GetAgentContactByAgentID(agentID).List[0], ReadAgentPersonalInformation.GetAgentPeronsalInformationByAgentID(agentID).List[0], ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(agentID)));
            }
            Agents selectedAgent = new Agents();
            foreach (Agent agent in allAgents.List)
            {
                if (agent.AgentUsername == userName)
                {
                    selectedAgent.Add(agent);
                }
            }
            return selectedAgent;
        }


	}
}
