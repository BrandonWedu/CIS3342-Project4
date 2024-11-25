using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public class WriteAgentPersonalInformation
    {
        internal static int CreateNew(AgentPersonalInformation info)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewAgentPersonalInformation";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@AgentID", info.AgentID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@FirstName", info.FirstName, SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@LastName", info.LastName, SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<byte[]>("@PersonalAddress", Serializer.SerializeData<Address>(info.PersonalAddress), SqlDbType.VarBinary));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@PersonalPhoneNumber", info.PhoneNumber, SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@PersonalEmail", info.Email, SqlDbType.VarChar, 50));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@AgentInfoID", SqlDbType.Int, 8);
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
