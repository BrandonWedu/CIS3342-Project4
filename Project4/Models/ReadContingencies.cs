using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public class ReadContingencies
    {
        private DBConnect databaseHandler = new DBConnect();
        private Contingencies allContingencies = new Contingencies();

        internal static Contingencies ReadAllContingencies()
        {

            DBConnect databaseHandler = new DBConnect();
            Contingencies allContingencies = new Contingencies();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectAllContingencies";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allContingencies.Add(new Contingency((int)row["ContingencyID"], (int)row["OfferID"], row["Contingency"].ToString()));
            }

            return allContingencies;
        }


        internal static Contingencies GetContingenciesByOfferID(int id)
        {
            DBConnect databaseHandler = new DBConnect();
            Contingencies allContingencies = new Contingencies();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectAllContingencies";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allContingencies.Add(new Contingency((int)row["ContingencyID"], (int)row["OfferID"], row["Contingency"].ToString()));
            }

            Contingencies selectedContingencies = new Contingencies();
            foreach (Contingency currentContingency in allContingencies.List)
            {
                if (currentContingency.OfferID == id)
                {
                    selectedContingencies.Add(currentContingency);
                }
            }
            return selectedContingencies;
        }
    }
}
