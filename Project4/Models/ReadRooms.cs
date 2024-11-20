using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    internal static class ReadRooms
    {
        internal static Rooms GetByHomeID(int homeID)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_GetRoomByHomeID";

            //add parameter
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", homeID, SqlDbType.Int, 8));

            Rooms homeRooms = new Rooms();

            //run sql
            DataSet dataSet = dbConnect.GetDataSet(sqlCommand);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                    homeRooms.Add(new Room
                        (
                            (int?)row["RoomID"],
                            (RoomType)Enum.Parse(typeof(RoomType), (string)row["RoomType"]),
                            (int)row["Height"],
                            (int)row["Width"]
                        ));
            }
            return homeRooms;
        }
    }
}
