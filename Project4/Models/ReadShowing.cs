using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    internal static class ReadShowing
    {
        internal static Showings GetShowingsByAgentID(int agentID)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_GetShowingsByAgentID";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@agentID", agentID, SqlDbType.Int, 8));

            DataSet dataSet = dbConnect.GetDataSet(sqlCommand);
            Showings showings = new Showings();

            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                //foreach (DataRow row in dataSet.Tables[0].Rows)
                //{
                    //showings.Add(new Showing
                    //(
                        //(int)row["ShowingID"],
                        //ReadHome.GetHomeByID((int)row["HomeID"]),
                        //ReadClient.GetClientByID((int)row["ClientID"]),
                        //(DateTime)row["TimeRequestCreated"],
                        //(DateTime)row["ShowingTime"],
                        //(ShowingStatus)Enum.Parse(typeof(ShowingStatus), (string)row["ShowingStatus"])
                    //));
                //}
            }
            return showings;
        }
    }
}
