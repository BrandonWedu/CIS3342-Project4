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
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@AgentPasswordSalt", agent.AgentPassword, SqlDbType.VarChar));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@AgentID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            Console.WriteLine("Executing stored procedure with parameters:");
            foreach (SqlParameter param in sqlCommand.Parameters)
            {
                Console.WriteLine($"{param.ParameterName}: {param.Value}");
            }

            dbConnect.DoUpdate(sqlCommand);

            if (outputParam.Value == DBNull.Value || outputParam.Value == null)
            {
                throw new InvalidOperationException("Failed to retrieve the new AgentID.");
            }

            return (int)outputParam.Value;
        }
    }
}
