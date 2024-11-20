using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class ReadContingencies
	{
		private DBConnect databaseHandler = new DBConnect();
		private ContingencyList allContingencies = new ContingencyList();

		public ReadContingencies()
		{
			allContingencies = new ContingencyList();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "SelectAllContingencies";
			DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentContactData.Rows)
			{
				allContingencies.Add(new Contingency((int)row["ContingencyID"], (int)row["OfferID"], row["Contingency"].ToString()));
			}
		}

		public ContingencyList GetAllOffers()
		{
			return allContingencies;
		}

		public ContingencyList GetContingenciesByOfferID(int id)
		{
			ContingencyList selectedContingencies = new ContingencyList();
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
