using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
    public class ReadRooms
    {
        internal static Rooms ReadAllRooms()
        {
            DBConnect databaseHandler = new DBConnect();
            Rooms allRooms = new Rooms();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllRooms";
            DataTable roomData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in roomData.Rows)
            {
                allRooms.Add(new Room((int)row["RoomID"], Enum.Parse<RoomType>(row["RoomType"].ToString()), (int)row["Height"], (int)row["Width"]));
            }

            return allRooms;
        }

        internal static Rooms GetRoomsByHomeID(int id)
        {
            DBConnect databaseHandler = new DBConnect();
            Rooms allRooms = new Rooms();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllRooms";
            DataTable roomData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in roomData.Rows)
            {
                if ((int)row["HomeID"] == id)
                {
                    allRooms.Add(new Room((int)row["RoomID"], Enum.Parse<RoomType>(row["RoomType"].ToString()), (int)row["Height"], (int)row["Width"]));
                }

            }

            return allRooms;
        }
    }
}
