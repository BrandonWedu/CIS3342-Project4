using System.Data;
using Microsoft.Data.SqlClient;

namespace HomeListingAPI
{
    internal static class WriteHome
    {
        internal static int CreateNew(Home home)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewHome";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@agentID", (int)home.Agent.AgentID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@cost", home.Cost, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<byte[]>("@homeAddress", Serializer.SerializeData<Address>(home.Address), SqlDbType.VarBinary));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@propertyType", home.PropertyType.ToString(), SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeSize", home.HomeSize, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@constructionYear", home.YearConstructed, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@garage", home.GarageType.ToString(), SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@homeDescription", home.Description, SqlDbType.VarChar));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<DateTime>("@dateListed", home.DateListed, SqlDbType.DateTime));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@saleStatus", home.SaleStatus.ToString(), SqlDbType.VarChar, 50));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@homeID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);
            return (int)outputParam.Value;
        }
        internal static bool UpdateSaleStatus(Home home)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_UpdateHomeSaleStatus";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", (int)home.HomeID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@saleStatus", home.SaleStatus.ToString(), SqlDbType.VarChar, 50));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@changed", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);
            return (int)outputParam.Value == 0;
        }
        internal static bool DeleteHome(int homeID)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_DeleteHome";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", homeID, SqlDbType.Int, 8));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@deleted", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);
            return (int)outputParam.Value == 0;

        }
    }
}
