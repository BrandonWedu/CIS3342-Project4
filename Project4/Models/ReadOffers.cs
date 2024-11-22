using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public class ReadOffers
    {

        internal static Offers ReadAllOffers()
        {

            DBConnect databaseHandler = new DBConnect();
            Offers allOffers = new Offers();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectAllOffers";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allOffers.Add(new Offer((int)row["OfferID"], ReadHome.GetHomeByID((int)row["HomeID"]), ReadClient.GetClientByID((int)row["ClientID"]), (decimal)row["Amount"], (TypeOfSale)Enum.Parse(typeof(TypeOfSale), (string)row["TypeOfSale"]), (bool)row["SellHomePrior"], (DateTime)row["MoveInDate"], (OfferStatus)Enum.Parse(typeof(OfferStatus), (string)row["Status"])));
            }
            return allOffers;
        }

        internal static Offers GetOfferByOfferID(int id)
        {
            DBConnect databaseHandler = new DBConnect();
            Offers allOffers = new Offers();
            Offers selectedOffer = new Offers();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectAllOffers";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allOffers.Add(new Offer((int)row["OfferID"], ReadHome.GetHomeByID((int)row["HomeID"]), ReadClient.GetClientByID((int)row["ClientID"]), (decimal)row["Amount"], (TypeOfSale)Enum.Parse(typeof(TypeOfSale), (string)row["TypeOfSale"]), (bool)row["SellHomePrior"], (DateTime)row["MoveInDate"], (OfferStatus)Enum.Parse(typeof(OfferStatus), (string)row["Status"])));
            }

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
