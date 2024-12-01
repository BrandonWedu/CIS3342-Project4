using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
	public class ReadTemperatureControl
	{
		internal static TemperatureControl ReadHomeTemperatureControl(int id)
		{
			DBConnect databaseHandler = new DBConnect();
			TemperatureControl control = null;
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "P4_SelectAllTemperatureControl";
			DataTable temperatureData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in temperatureData.Rows)
			{
				if ((int)row["HomeID"] == id)
				{
					control = new TemperatureControl((int)row["TemperatureControlID"], Enum.Parse<HeatingTypes>(row["Heat"].ToString()), Enum.Parse<CoolingTypes>(row["Cool"].ToString()));
				}
			}

			return control;
		}
	}
}
