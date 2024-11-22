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
    }
}
