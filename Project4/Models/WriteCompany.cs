using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public class WriteCompany
    {
        internal static int CreateNew(Company company)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewCompany";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@CompanyName", company.CompanyName, SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<byte[]>("@CompanyAddress", Serializer.SerializeData<Address>(company.CompanyAddress), SqlDbType.VarBinary));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@CompanyPhoneNumber", company.PhoneNumber, SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@CompanyEmail", company.Email, SqlDbType.VarChar, 50));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@CompanyID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);

            return (int)outputParam.Value;
        }
    }
}
