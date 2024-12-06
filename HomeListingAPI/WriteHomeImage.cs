using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
    internal static class WriteHomeImage
    {
        internal static int CreateNew(int homeID, Image homeImage)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewHomeImage";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", (int)homeID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@imageURL", homeImage.Url, SqlDbType.VarChar));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@imageLocation", homeImage.Type.ToString(), SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@imageDescription", homeImage.Description, SqlDbType.VarChar));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<bool>("@mainImage", homeImage.MainImage, SqlDbType.Bit));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@imageID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);
            return (int)outputParam.Value;
        }

        internal static void DeleteHomeImage(int homeID)
        {


			try
			{
				DBConnect dbConnect = new DBConnect();
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "P4_DeleteHomeImage";
				sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@HomeID", (int)homeID, SqlDbType.Int, 8));
				dbConnect.DoUpdate(sqlCommand);
			}
			catch (SqlException sqlEx)
			{
				// Handle SQL specific exceptions, like issues with query, constraints, etc.
				Console.WriteLine($"SQL Exception: {sqlEx.Message}");
				throw; // Rethrow if you want it to bubble up or handle it differently
			}
			catch (Exception ex)
			{
				// Handle all other types of exceptions
				Console.WriteLine($"Exception: {ex.Message}");
				throw; // Rethrow the exception to be handled at a higher level or log it
			}
		}

        internal static void UpdateHomeImages(int homeID, Images updatedImages, Images oldImages)
        {
            //Update or add images
            foreach (Image currentImage in updatedImages.List)
            {
                if (currentImage.ImageID != 0 || currentImage.ImageID != null)
                {
                    DBConnect dbConnect = new DBConnect();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "P4_UpdateHomeImage";
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@ImageID", (int)currentImage.ImageID, SqlDbType.Int, 8));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@HomeID", homeID, SqlDbType.Int, 8));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@ImageURL", currentImage.Url, SqlDbType.VarChar));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@ImageLocation", currentImage.Type.ToString(), SqlDbType.VarChar, 50));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@ImageDescription", currentImage.Description, SqlDbType.VarChar));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<bool>("@MainImage", currentImage.MainImage, SqlDbType.Bit));
                    dbConnect.DoUpdate(sqlCommand);
                }
                else
                {
                    //Create new images
                    DBConnect dbConnect = new DBConnect();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "P4_CreateNewHomeImage";
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", (int)homeID, SqlDbType.Int, 8));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@imageURL", currentImage.Url, SqlDbType.VarChar));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@imageLocation", currentImage.Type.ToString(), SqlDbType.VarChar, 50));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@imageDescription", currentImage.Description, SqlDbType.VarChar));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<bool>("@mainImage", currentImage.MainImage, SqlDbType.Bit));
                    SqlParameter outputParam = DBParameterHelper.OutputParameter("@imageID", SqlDbType.Int, 8);
                    sqlCommand.Parameters.Add(outputParam);
                    dbConnect.DoUpdate(sqlCommand);
                }
            }

            //Remove deleted images
            foreach (Image currentOldImage in oldImages.List)
            {
                foreach (Image currentImage in updatedImages.List)
                {
                    // Every updated images gets compared to each old image
                    if (currentOldImage.ImageID != currentImage.ImageID)
                    {
                        //Delete the old image because it wasnt present in the updated images 
                        DBConnect dbConnect = new DBConnect();
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "P4_RemoveImage";
                        sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@RoomID", (int)currentOldImage.ImageID, SqlDbType.Int, 8));
                        dbConnect.DoUpdate(sqlCommand);
                    }
                }
            }


        }
    }
}
