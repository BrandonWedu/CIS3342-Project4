using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class WriteContingencies
	{
		private DBConnect databaseHandler = new DBConnect();

		public void CreateNewContingencies(ContingencyList contingencies, int offerID)
		{
			foreach (Contingency currentContingency in contingencies.List)
			{


				SqlCommand insertProcedure = new SqlCommand();
				insertProcedure.CommandType = CommandType.StoredProcedure;
				insertProcedure.CommandText = "InsertNewContingency";
				insertProcedure.Parameters.AddWithValue("@OfferID", offerID);
				insertProcedure.Parameters.AddWithValue("@Contingency", currentContingency.Contingecny);
				databaseHandler.DoUpdate(insertProcedure);
			}
		}

		public void RemoveContingencies(int offerID)
		{
			SqlCommand deleteProcedure = new SqlCommand();
			deleteProcedure.CommandType = CommandType.StoredProcedure;
			deleteProcedure.CommandText = "DeleteContingencies";
			deleteProcedure.Parameters.AddWithValue("@OfferID", offerID);
			databaseHandler.DoUpdate(deleteProcedure);
		}
	}
}
