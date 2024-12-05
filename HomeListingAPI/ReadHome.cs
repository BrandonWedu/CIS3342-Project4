using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
    public class ReadHome
    {
        internal static Homes ReadAllHomes()
        {

            DBConnect databaseHandler = new DBConnect();
            Homes allHomes = new Homes();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllHomes";
            DataTable homeData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in homeData.Rows)
            {
                int agentID = (int)row["AgentID"];
                int homeID = (int)row["HomeID"];
                allHomes.Add(new Home((int)row["HomeID"], agentID, (int)row["Cost"], Serializer.DeserializeData<Address>((byte[])row["HomeAddress"]), Enum.Parse<PropertyType>(row["PropertyType"].ToString()), (int)row["ConstructionYear"], Enum.Parse<GarageType>(row["Garage"].ToString()), row["HomeDescription"].ToString(), (DateTime)row["DateListed"], Enum.Parse<SaleStatus>(row["SaleStatus"].ToString()), ReadImages.GetImagesByHomeID(homeID), ReadAmenities.GetAmenitiesByHomeID(homeID), ReadTemperatureControl.ReadHomeTemperatureControl(homeID), ReadRooms.GetRoomsByHomeID(homeID), ReadUtilities.GetUtilitesByHomeID(homeID)));
            }

            return allHomes;

        }

        internal static Home GetHomeByHomeID(int id)
        {
            DBConnect databaseHandler = new DBConnect();
            Home selectedHome = null;
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllHomes";
            DataTable homeData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in homeData.Rows)
            {
                if ((int)row["HomeID"] == id)
                {
                    int agentID = (int)row["AgentID"];
                    int homeID = (int)row["HomeID"];
                    selectedHome = new Home((int)row["HomeID"], agentID, (int)row["Cost"], Serializer.DeserializeData<Address>((byte[])row["HomeAddress"]), Enum.Parse<PropertyType>(row["PropertyType"].ToString()), (int)row["ConstructionYear"], Enum.Parse<GarageType>(row["Garage"].ToString()), row["HomeDescription"].ToString(), (DateTime)row["DateListed"], Enum.Parse<SaleStatus>(row["SaleStatus"].ToString()), ReadImages.GetImagesByHomeID(homeID), ReadAmenities.GetAmenitiesByHomeID(homeID), ReadTemperatureControl.ReadHomeTemperatureControl(homeID), ReadRooms.GetRoomsByHomeID(homeID), ReadUtilities.GetUtilitesByHomeID(homeID));
                }

            }

            return selectedHome;
        }
    }
}
