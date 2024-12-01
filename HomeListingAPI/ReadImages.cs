using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
	public class ReadImages
	{
		internal static Images ReadAllImages()
		{
			DBConnect databaseHandler = new DBConnect();
			Images allImages = new Images();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "P4_SelectAllImages";
			DataTable imageData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in imageData.Rows)
			{
				allImages.Add(new Image((int)row["HomeImageID"], row["ImageUrl"].ToString(), Enum.Parse<RoomType>(row["ImageLocation"].ToString()), row["ImageDescription"].ToString(), (bool)row["MainImage"]));
			}

			return allImages;
		}

		internal static Images GetImagesByHomeID(int id)
		{
			DBConnect databaseHandler = new DBConnect();
			Images allImages = new Images();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "P4_SelectAllImages";
			DataTable imageData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in imageData.Rows)
			{
				if ((int)row["HomeID"] == id)
				{
					allImages.Add(new Image((int)row["HomeImageID"], row["ImageUrl"].ToString(), Enum.Parse<RoomType>(row["ImageLocation"].ToString()), row["ImageDescription"].ToString(), (bool)row["MainImage"]));
				}
			}
			return allImages;
		}

	}
}
