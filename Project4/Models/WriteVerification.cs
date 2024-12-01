using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public static class WriteVerification
    {
        internal static bool CreateNew(int agentID, int code)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewVerification";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@agentID", agentID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@code", code, SqlDbType.Int, 8));

            dbConnect.DoUpdate(sqlCommand);

            return true;
        }


        internal static void Verify(int agentID)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_AgentVerify";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@agentID", agentID, SqlDbType.Int, 8));

            dbConnect.DoUpdate(sqlCommand);
        }
    }
}
