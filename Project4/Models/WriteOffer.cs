using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class WriteOffer
	{
		private DBConnect databaseHandler = new DBConnect();
		public void CreateNewOffer(Offer newOffer)
		{
			SqlCommand insertProcedure = new SqlCommand();
			insertProcedure.CommandType = CommandType.StoredProcedure;
			insertProcedure.CommandText = "InsertNewOffer";
			insertProcedure.Parameters.AddWithValue("@HomeID", newOffer.Home.HomeID);
			insertProcedure.Parameters.AddWithValue("@ClientID", newOffer.Client.ClientID);
			insertProcedure.Parameters.AddWithValue("@Amount", newOffer.Amount);
			insertProcedure.Parameters.AddWithValue("@TypeOfSale", newOffer.TypeOfSale);
			insertProcedure.Parameters.AddWithValue("@SellHomePrior", newOffer.SellHomePrior);
			insertProcedure.Parameters.AddWithValue("@MoveInDate", newOffer.MoveInDate);
			insertProcedure.Parameters.AddWithValue("@Status", newOffer.Status);
			databaseHandler.DoUpdate(insertProcedure);
		}

		public void UpdateOffer(Offer offer, OfferStatus newStatus)
		{
			SqlCommand updateProcedure = new SqlCommand();
			updateProcedure.CommandType = CommandType.StoredProcedure;
			updateProcedure.CommandText = "UpdateOfferStatus";
			updateProcedure.Parameters.AddWithValue("@OfferID", offer.OfferID);
			updateProcedure.Parameters.AddWithValue("@Status", newStatus);
			databaseHandler.DoUpdate(updateProcedure);
		}

		public void RemoveOffer(Offer offer)
		{
			SqlCommand deleteProcedure = new SqlCommand();
			deleteProcedure.CommandType = CommandType.StoredProcedure;
			deleteProcedure.CommandText = "DeleteOffer";
			deleteProcedure.Parameters.AddWithValue("@OfferID", offer.OfferID);
			databaseHandler.DoUpdate(deleteProcedure);
		}
	}
}
