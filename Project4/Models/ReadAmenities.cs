using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    internal static class ReadAmenities
    {
        internal static Amenities GetByHomeID(int homeID)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_GetAmenityByHomeID";

            //add parameter
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", homeID, SqlDbType.Int, 8));

            Amenities homeAmenities = new Amenities();

            //run sql
            DataSet dataSet = dbConnect.GetDataSet(sqlCommand);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                    homeAmenities.Add(new Amenity
                        (
                            (int?)row["AmenityID"],
                            (AmenityType)Enum.Parse(typeof(AmenityType), (string)row["AmenityType"]),
                            (string)row["AmenityDescription"]
                        ));
            }
            return homeAmenities;
        }
    }
}
