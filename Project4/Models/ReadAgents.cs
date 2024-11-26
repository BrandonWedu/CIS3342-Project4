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
            sqlCommand.CommandText = "P4_SelectAllAgents";
            DataTable agentData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentData.Rows)
            {
                int agentID = (int)row["AgentID"];
                int companyID = (int)row["CompanyID"];
                allAgents.Add(new Agent((int)row["AgentID"], row["AgentUsername"].ToString(), row["AgentPassword"].ToString(), row["AgentPasswordSalt"].ToString(), (bool)row["AgentVerified"], ReadCompanies.GetCompanyByCompanyID(companyID).List[0], ReadAgentContact.GetAgentContactByAgentID(agentID).List[0], ReadAgentPersonalInformation.GetAgentPeronsalInformationByAgentID(agentID).List[0], ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(agentID)));
            }

            return allAgents;
        }


        internal static Agent GetAgentByAgentID(int id)
        {
            DBConnect databaseHandler = new DBConnect();
            Agents allAgents = new Agents();
            Agent selectedAgent = null;
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllAgents";
            DataTable agentData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentData.Rows)
            {
                if ((int)row["AgentID"] == id)
                {
                    int agentID = (int)row["AgentID"];
                    int companyID = (int)row["CompanyID"];
                    selectedAgent = new Agent((int)row["AgentID"], row["AgentUsername"].ToString(), row["AgentPassword"].ToString(), row["AgentPasswordSalt"].ToString(), (bool)row["AgentVerified"], ReadCompanies.GetCompanyByCompanyID(companyID).List[0], ReadAgentContact.GetAgentContactByAgentID(agentID).List[0], ReadAgentPersonalInformation.GetAgentPeronsalInformationByAgentID(agentID).List[0], ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(agentID));
                }

            }

            return selectedAgent;
        }

        internal static Agent GetAgentByUsername(string username)
        {
            DBConnect databaseHandler = new DBConnect();
            Agents allAgents = new Agents();
            Agent selectedAgent = null;
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllAgents";
            DataTable agentData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentData.Rows)
            {
                if (row["AgentUsername"].ToString() == username)
                {
                    int agentID = (int)row["AgentID"];
                    int companyID = (int)row["CompanyID"];
                    selectedAgent = new Agent((int)row["AgentID"], row["AgentUsername"].ToString(), row["AgentPassword"].ToString(), row["AgentPasswordSalt"].ToString(), (bool)row["AgentVerified"], ReadCompanies.GetCompanyByCompanyID(companyID).List[0], ReadAgentContact.GetAgentContactByAgentID(agentID).List[0], ReadAgentPersonalInformation.GetAgentPeronsalInformationByAgentID(agentID).List[0], ReadAgentSecurity.GetAgentSecurityQuestionsByAgentID(agentID));

                }
            }

            return selectedAgent;
        }

        internal static int GetAgentIDByUsername(string userName)
        {
            DBConnect databaseHandler = new DBConnect();
            Agents allAgents = new Agents();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAgentIDByUsername";
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@AgentUsername", userName, SqlDbType.VarChar, 50));
            DataTable agentData = databaseHandler.GetDataSet(sqlCommand).Tables[0];
            int agentID = 0;
            foreach (DataRow row in agentData.Rows)
            {
                agentID = (int)row["AgentID"];
            }
            return agentID;
        }


    }
}
