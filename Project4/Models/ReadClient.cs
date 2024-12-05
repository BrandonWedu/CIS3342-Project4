using Microsoft.Data.SqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace Project4.Models
{
    internal static class ReadClient
    {
        internal static Client GetClientByID(int clientID)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_GetClientByID";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@clientID", clientID, SqlDbType.Int, 8));

            DataSet dataSet = dbConnect.GetDataSet(sqlCommand);
            DataRow row = dataSet.Tables[0].Rows[0];
            return new Client
                (
                clientID,
                (string)row["FirstName"],
                (string)row["LastName"],
                Serializer.DeserializeData<Address>((byte[])row["ClientAddress"]),
                (string)row["PhoneNumber"],
                (string)row["Email"]
                );
        }
    }
}
