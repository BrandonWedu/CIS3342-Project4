using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    internal static class ReadImages
    {
        internal static Images GetByHomeID(int homeID)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_GetHomeImagesByHomeID";

            //add parameter
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", homeID, SqlDbType.Int, 8));

            Images homeImages = new Images();

            //run sql
            DataSet dataSet = dbConnect.GetDataSet(sqlCommand);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                    homeImages.Add(new Image
                        (
                            (int?)row["HomeImageID"],
                            (string)row["ImageURL"],
                            (RoomType)Enum.Parse(typeof(RoomType), (string)row["ImageLocation"]),
                            (string)row["ImageDescription"],
                            (bool)row["MainImage"]
                        ));
            }
            return homeImages;
        }
    }
}
