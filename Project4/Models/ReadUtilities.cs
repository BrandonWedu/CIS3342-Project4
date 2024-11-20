using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    internal static class ReadUtilities
    {
        internal static Utilities GetByHomeID(int homeID)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "GetUtilityByHomeID";

            //add parameter
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", homeID, SqlDbType.Int, 8));

            Utilities homeUtilities = new Utilities();

            //run sql
            DataSet dataSet = dbConnect.GetDataSet(sqlCommand);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                    homeUtilities.Add(new Utility
                        (
                            (int?)row["UtilityId"],
                            (UtilityTypes)Enum.Parse(typeof(UtilityTypes), (string)row["UtilityType"]),
                            (string)row["UtilityInformation"]
                        ));
            }
            return homeUtilities;
        }
    }
}
