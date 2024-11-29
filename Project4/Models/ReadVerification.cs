using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public static class ReadVerification
    {
        internal static int GetVerifiedCode(int agentID)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_GetVerificationCode";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@agentID", agentID, SqlDbType.Int, 8));

            SqlParameter code = sqlCommand.Parameters.Add(DBParameterHelper.OutputParameter("@code", SqlDbType.Int, 8));

            dbConnect.GetDataSet(sqlCommand);

            return (int)code.Value;
        }
    }
}
