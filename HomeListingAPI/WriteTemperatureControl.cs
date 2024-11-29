using System.Data;
using Microsoft.Data.SqlClient;

namespace HomeListingAPI
{
    internal static class WriteTemperatureControl
    {
        internal static int CreateNew(int homeID, TemperatureControl temperatureControl)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewTemperatureControl";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", (int)homeID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@cool", temperatureControl.Cooling.ToString(), SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@heat", temperatureControl.Heating.ToString(), SqlDbType.VarChar, 50));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@temperatureControlID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);
            return (int)outputParam.Value;
        }

        internal static void UpdateTemperatureControl(int homeID, TemperatureControl updatedControl)
        {
			DBConnect dbConnect = new DBConnect();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "P4_UpdateTemperatureControl";

			sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@TemperatureControlID", (int)updatedControl.TemperatureControlID, SqlDbType.Int, 8));
			sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@HomeID", homeID, SqlDbType.Int, 8));
			sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@Cool", updatedControl.Cooling.ToString(), SqlDbType.VarChar, 50));
			sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@Heat", updatedControl.Heating.ToString(), SqlDbType.VarChar, 50));
			dbConnect.DoUpdate(sqlCommand);
		}
    }
}
