using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public class WriteContingencies
    {
        internal static void CreateNew(Contingencies newContingencies)
        {

            foreach (Contingency currentContingency in newContingencies.List)
            {
                DBConnect dbConnect = new DBConnect();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "P4_CreateNewContingency";

                sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@OfferID", (int)currentContingency.OfferID, SqlDbType.Int, 8));
                sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@Contingency", currentContingency.OfferContingency, SqlDbType.VarChar, -1));


                dbConnect.DoUpdate(sqlCommand);
            }

        }
    }
}
