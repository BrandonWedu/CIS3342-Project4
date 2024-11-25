using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public class WriteAgentContact
    {
        internal static int CreateNew(AgentContact contact)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewAgentContact";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@AgentID", contact.AgentID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<byte[]>("@WorkAddress", Serializer.SerializeData<Address>(contact.WorkAddress), SqlDbType.VarBinary));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@WorkPhoneNumber", contact.PhoneNumber, SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@WorkEmail", contact.Email, SqlDbType.VarChar, 50));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@AgentContactID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);

            return (int)outputParam.Value;
        }
    }
}
