using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public class WriteAgent
    {
        internal static int CreateNew(Agent agent)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewAgent";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@CompanyID", agent.WorkCompany.CompanyID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@AgentUsername", agent.AgentUsername, SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@AgentPassword", agent.AgentPassword, SqlDbType.VarChar));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@AgentPasswordSalt", agent.AgentPasswordSalt, SqlDbType.VarChar));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@AgentID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);


            dbConnect.DoUpdate(sqlCommand);

            if (outputParam.Value == DBNull.Value || outputParam.Value == null)
            {
                throw new InvalidOperationException("Failed to retrieve the new AgentID.");
            }

            return (int)outputParam.Value;
        }

        internal static void UpdateAgentPassword(Agent agent, string hashedPW, string salt)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_UpdateAgentPassword";
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@AgentID", agent.AgentID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@AgentPassword", hashedPW, SqlDbType.VarChar));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@AgentPasswordSalt", salt, SqlDbType.VarChar));
            dbConnect.DoUpdate(sqlCommand);
        }
    }
}
