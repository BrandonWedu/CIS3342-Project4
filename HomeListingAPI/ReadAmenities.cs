using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
	public class ReadAmenities
	{
		internal static Amenities ReadAllAmenitites()
		{
			DBConnect databaseHandler = new DBConnect();
			Amenities allAmenities = new Amenities();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "P4_SelectAllAmenities";
			DataTable amenityData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in amenityData.Rows)
			{
				allAmenities.Add(new Amenity((int)row["AmenityID"], Enum.Parse<AmenityType>(row["AmenityType"].ToString()), row["AmenityDescription"].ToString()));
			}

			return allAmenities;
		}

		internal static Amenities GetAmenitiesByHomeID(int id)
		{
			DBConnect databaseHandler = new DBConnect();
			Amenities allAmenities = new Amenities();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "P4_SelectAllAmenities";
			DataTable amenityData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in amenityData.Rows)
			{
				if ((int)row["HomeID"] == id)
				{
					allAmenities.Add(new Amenity((int)row["AmenityID"], Enum.Parse<AmenityType>(row["AmenityType"].ToString()), row["AmenityDescription"].ToString()));
				}
				
			}

			return allAmenities;
		}
	}
}
