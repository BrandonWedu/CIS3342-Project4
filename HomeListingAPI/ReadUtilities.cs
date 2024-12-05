using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
    public class ReadUtilities
    {
        internal static Utilities ReadAllUtilites()
        {
            DBConnect databaseHandler = new DBConnect();
            Utilities allUtilties = new Utilities();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllUtilites";
            DataTable utilitesData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in utilitesData.Rows)
            {
                allUtilties.Add(new Utility((int)row["UtilityID"], Enum.Parse<UtilityTypes>(row["UtilityType"].ToString()), row["UtilityInformation"].ToString()));
            }

            return allUtilties;
        }

        internal static Utilities GetUtilitesByHomeID(int id)
        {
            DBConnect databaseHandler = new DBConnect();
            Utilities allUtilties = new Utilities();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllUtilites";
            DataTable utilitesData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in utilitesData.Rows)
            {
                if ((int)row["HomeID"] == id)
                {
                    allUtilties.Add(new Utility((int)row["UtilityID"], Enum.Parse<UtilityTypes>(row["UtilityType"].ToString()), row["UtilityInformation"].ToString()));
                }

            }

            return allUtilties;
        }
    }
}
