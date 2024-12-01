using System.Data;
using Microsoft.Data.SqlClient;

namespace HomeListingAPI
{
    internal static class WriteRoom
    {
        internal static int CreateNew(int homeID, Room room)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewRoom";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", (int)homeID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@roomType", room.Type.ToString(), SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@height", room.Height, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@width", room.Width, SqlDbType.Int, 8));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@roomID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);
            return (int)outputParam.Value;
        }

        internal static void UpdateRooms(int homeID, Rooms updatedRooms)
        {
            foreach(Room currentRoom in updatedRooms.List)
            {
				DBConnect dbConnect = new DBConnect();
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "P4_UpdateRoom";
				sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@RoomID", (int)currentRoom.RoomID, SqlDbType.Int, 8));
				sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@HomeID", homeID, SqlDbType.Int, 8));
				sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@RoomType", currentRoom.Type.ToString(), SqlDbType.VarChar, 50));
				sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@Height", currentRoom.Height, SqlDbType.Int, 8));
				sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@Width", currentRoom.Width, SqlDbType.Int, 8));
			}
        }
    }
}
