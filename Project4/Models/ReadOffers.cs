using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class ReadOffers
	{
		private DBConnect databaseHandler = new DBConnect();
		private OfferList allOffers = new OfferList();

		public ReadOffers()
		{
			allOffers = new OfferList();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "SelectAllOffers";
			DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentContactData.Rows)
			{
				allOffers.Add(new Offer((int)row["OfferID"], ReadHome.GetHomeByID((int)row["HomeID"]), ReadClient.GetClientByID((int)row["ClientID"]), (decimal)row["Amount"], (TypeOfSale)Enum.Parse(typeof(TypeOfSale), (string)row["TypeOfSale"]), (bool)row["SellHomePrior"], (DateTime)row["MoveInDate"], (OfferStatus)Enum.Parse(typeof(OfferStatus), (string)row["Status"])));
			}
		}

		public OfferList GetAllOffers()
		{
			return allOffers;
		}

		public OfferList GetOfferByOfferID(int id)
		{
			OfferList selectedOffer = new OfferList();
			foreach (Offer currentOffer in allOffers.List)
			{
				if (currentOffer.OfferID == id)
				{
					selectedOffer.Add(currentOffer);
				}
			}
			return selectedOffer;
		}
	}
}
