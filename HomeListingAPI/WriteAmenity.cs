using System.Data;
using Microsoft.Data.SqlClient;

namespace HomeListingAPI
{
    internal class WriteAmenity
    {
        internal static int CreateNew(int homeID, Amenity amenity)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewAmenity";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", (int)homeID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@amenityType", amenity.Type.ToString(), SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@amenityDescription", amenity.Description, SqlDbType.VarChar));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@amenityID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);
            return (int)outputParam.Value;
        }

        internal static void UpdateAmenities(int homeID, Amenities updatedAmenities, Amenities oldAmenities)
        {
            foreach (Amenity currentAmenity in updatedAmenities.List)
            {
                if (currentAmenity.AmenityID != 0 || currentAmenity.AmenityID != null)
                {
                    DBConnect dbConnect = new DBConnect();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "P4_UpdateAmenity";
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@AmenityID", (int)currentAmenity.AmenityID, SqlDbType.Int, 8));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@HomeID", homeID, SqlDbType.Int, 8));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@AmenityType", currentAmenity.Type.ToString(), SqlDbType.VarChar, 50));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@AmenityDescription", currentAmenity.Description, SqlDbType.VarChar));
                    dbConnect.DoUpdate(sqlCommand);
                }
                else
                {
                    //Create new amenities
                    DBConnect dbConnect = new DBConnect();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "P4_CreateNewAmenity";
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", (int)homeID, SqlDbType.Int, 8));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@amenityType", currentAmenity.Type.ToString(), SqlDbType.VarChar, 50));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@amenityDescription", currentAmenity.Description, SqlDbType.VarChar));
                    SqlParameter outputParam = DBParameterHelper.OutputParameter("@amenityID", SqlDbType.Int, 8);
                    sqlCommand.Parameters.Add(outputParam);
                    dbConnect.DoUpdate(sqlCommand);
                }
            }

            //Remove deleted amenities
            foreach (Amenity currentOldAmenity in oldAmenities.List)
            {
                foreach (Amenity currentAmenity in updatedAmenities.List)
                {
                    // Every updated amenity gets compared to each old amenity
                    if (currentOldAmenity.AmenityID != currentAmenity.AmenityID)
                    {
                        //Delete the old amenity because it wasnt present in the updated amenities
                        DBConnect dbConnect = new DBConnect();
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "P4_RemoveAmenity";
                        sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@AmenityID", (int)currentOldAmenity.AmenityID, SqlDbType.Int, 8));
                        dbConnect.DoUpdate(sqlCommand);
                    }
                }
            }

        }
    }
}
