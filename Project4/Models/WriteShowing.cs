using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    internal static class WriteShowing
    {
        internal static bool UpdateStatus(int showingID, ShowingStatus status)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_UpdateShowingStatus";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@showingID", showingID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@showingStatus", status.ToString(), SqlDbType.VarChar, 50));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@statusCode", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);

            //return -1 if unsuccessful
            return (int)outputParam.Value > -1;
        }
        internal static bool CreateNew(Showing showing)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewShowing";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@firstName", showing.Client.FirstName, SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@lastName", showing.Client.LastName, SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<byte[]>("@clientAddress", Serializer.SerializeData<Address>(showing.Client.ClientAddress), SqlDbType.VarBinary));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@clientPhoneNumber", showing.Client.PhoneNumber, SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@clientEmail", showing.Client.Email, SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", (int)showing.Home.HomeID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<DateTime>("@timeRequestCreated", showing.TimeRequestCreated, SqlDbType.DateTime));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<DateTime>("@showingTime", showing.ShowingTime, SqlDbType.DateTime));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@showingStatus", showing.Status.ToString(), SqlDbType.VarChar, 50));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@showingID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);

            return (int)outputParam.Value > -1;
        }
    }
}
