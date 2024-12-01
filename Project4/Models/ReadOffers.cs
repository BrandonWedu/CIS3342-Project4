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
                //allOffers.Add(new Offer((int)row["OfferID"], ReadHome.GetHomeByID((int)row["HomeID"]), ReadClient.GetClientByID((int)row["ClientID"]), (int)row["Amount"], (TypeOfSale)Enum.Parse(typeof(TypeOfSale), (string)row["TypeOfSale"]), (bool)row["SellHomePrior"], (DateTime)row["MoveInDate"], (OfferStatus)Enum.Parse(typeof(OfferStatus), (string)row["OfferStatus"])));
            }
            return allOffers;
        }

        internal static Offer GetOfferByOfferID(int id)
        {
            DBConnect databaseHandler = new DBConnect();
            Offers allOffers = new Offers();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectAllOffers";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];
            Offer selectedOffer = null;
            foreach (DataRow row in agentContactData.Rows)
            {
                if ((int)row["OfferID"] == id)
                {
                    //selectedOffer = new Offer(new Offer((int)row["OfferID"], ReadHome.GetHomeByID((int)row["HomeID"]), ReadClients.GetClientByID((int)row["ClientID"]), (int)row["Amount"], (TypeOfSale)Enum.Parse(typeof(TypeOfSale), (string)row["TypeOfSale"]), (bool)row["SellHomePrior"], (DateTime)row["MoveInDate"], (OfferStatus)Enum.Parse(typeof(OfferStatus), (string)row["OfferStatus"])));
                }
            }


            return selectedOffer;
        }


        internal static Offer GetOfferByHomeClientAmount(Home home, Client client, int amount)
        {
            DBConnect databaseHandler = new DBConnect();
            Offers allOffers = new Offers();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectAllOffers";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];
            Offer selectedOffer = null;
            foreach (DataRow row in agentContactData.Rows)
            {
                //Home rowHome = ReadHome.GetHomeByID((int)row["HomeID"]);
                //Client rowClient = ReadClients.GetClientByID((int)row["ClientID"])
                /*
                if ((int)row["Amount"] == amount && rowHome.HomeID == home.HomeID && rowClient == client.ClientID)
                {
                    //selectedOffer = new Offer(new Offer((int)row["OfferID"], ReadHome.GetHomeByID((int)row["HomeID"]), ReadClients.GetClientByID((int)row["ClientID"]), (int)row["Amount"], (TypeOfSale)Enum.Parse(typeof(TypeOfSale), (string)row["TypeOfSale"]), (bool)row["SellHomePrior"], (DateTime)row["MoveInDate"], (OfferStatus)Enum.Parse(typeof(OfferStatus), (string)row["OfferStatus"])));
                }
                */
            }


            return selectedOffer;
        }
    }
}
