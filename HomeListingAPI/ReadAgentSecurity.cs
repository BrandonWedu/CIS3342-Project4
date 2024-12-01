using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
    public class ReadAgentSecurity
    {

        internal static AgentSecuritys ReadAllAgentSecurity()
        {
            DBConnect databaseHandler = new DBConnect();
            AgentSecuritys allAgentSecurity = new AgentSecuritys();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAgentSecurity";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allAgentSecurity.Add(new AgentSecurity((int)row["SecurityQuestionsID"], (int)row["AgentID"], row["Question"].ToString(), row["Answer"].ToString()));
            }

            return allAgentSecurity;
        }


        internal static AgentSecuritys GetAgentSecurityQuestionsByAgentID(int id)
        {

            DBConnect databaseHandler = new DBConnect();
            AgentSecuritys allAgentSecurity = new AgentSecuritys();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAgentSecurity";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                if ((int)row["AgentID"] == id)
                {
                    allAgentSecurity.Add(new AgentSecurity((int)row["SecurityQuestionsID"], (int)row["AgentID"], row["Question"].ToString(), row["Answer"].ToString()));
                }
            }


            return allAgentSecurity;
        }
    }
}
