using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public class WriteAgentSecurityQuestion
    {
        internal static int CreateNew(AgentSecurity question)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewAgentSecurity";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@AgentID", question.AgentID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@Question", question.Question, SqlDbType.VarChar));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@Answer", question.Answer, SqlDbType.VarChar, 50));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@SecurityQuestionsID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);

            return (int)outputParam.Value;
        }
    }
}
