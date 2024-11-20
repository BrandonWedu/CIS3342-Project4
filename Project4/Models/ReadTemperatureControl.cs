using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    internal static class ReadTemperatureControl
    {
        internal static TemperatureControl GetByHomeID(int homeID)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_GetTemperatureControlByHomeID";

            //add parameter
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", homeID, SqlDbType.Int, 8));

            //run sql
            DataSet dataSet = dbConnect.GetDataSet(sqlCommand);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                DataRow row = dataSet.Tables[0].Rows[0];
                return new TemperatureControl
                (
                    (int?)row["TemperatureControlID"],
                    (HeatingTypes)Enum.Parse(typeof(HeatingTypes), (string)row["Heat"]),
                    (CoolingTypes)Enum.Parse(typeof(CoolingTypes), (string)row["Cool"])
                );
            }
            return null;
        }
    }
}
