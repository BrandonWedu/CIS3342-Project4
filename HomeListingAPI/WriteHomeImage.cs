using System.Data;
using Microsoft.Data.SqlClient;

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

        internal static void UpdateHomeImages(int homeID, Images updatedImages)
        {
            foreach (Image currentImage in updatedImages.List)
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


		}
    }
}
