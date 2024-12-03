using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
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
            sqlCommand.CommandText = "P4_SelectAllOffers";
            DataTable offerData = databaseHandler.GetDataSet(sqlCommand).Tables[0];


            foreach (DataRow row in offerData.Rows)
            {
                int homeID = (int)row["HomeID"];
                string apiUrl = $"https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadSingleHomeListing/{homeID}";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                string jsonString = response.Content.ReadAsStringAsync().Result;
                Home offerHome = JsonConvert.DeserializeObject<Home>(jsonString);
                allOffers.Add(new Offer((int)row["OfferID"], offerHome, ReadClients.GetClientByClientID((int)row["ClientID"]), (int)row["Amount"], (TypeOfSale)Enum.Parse(typeof(TypeOfSale), (string)row["TypeOfSale"]), (bool)row["SellHomePrior"], (DateTime)row["MoveInDate"], (OfferStatus)Enum.Parse(typeof(OfferStatus), (string)row["OfferStatus"])));
            }
            return allOffers;
        }

        internal static Offer GetOfferByOfferID(int id)
        {
            DBConnect databaseHandler = new DBConnect();
            Offers allOffers = new Offers();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllOffers";
            DataTable offerData = databaseHandler.GetDataSet(sqlCommand).Tables[0];
            Offer selectedOffer = null;
            foreach (DataRow row in offerData.Rows)
            {
                if ((int)row["OfferID"] == id)
                {
                    int homeID = (int)row["HomeID"];
                    string apiUrl = $"https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadSingleHomeListing/{homeID}";
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                    string jsonString = response.Content.ReadAsStringAsync().Result;
                    Home offerHome = JsonConvert.DeserializeObject<Home>(jsonString);
                    selectedOffer = new Offer((int)row["OfferID"], offerHome, ReadClients.GetClientByClientID((int)row["ClientID"]), (int)row["Amount"], (TypeOfSale)Enum.Parse(typeof(TypeOfSale), (string)row["TypeOfSale"]), (bool)row["SellHomePrior"], (DateTime)row["MoveInDate"], (OfferStatus)Enum.Parse(typeof(OfferStatus), (string)row["OfferStatus"]));
                }
            }


            return selectedOffer;
        }


        internal static Offer GetOfferByHomeClientAmount(Home home, Client offerClient, int amount)
        {
            DBConnect databaseHandler = new DBConnect();
            Offers allOffers = new Offers();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllOffers";
            DataTable offerData = databaseHandler.GetDataSet(sqlCommand).Tables[0];
            Offer selectedOffer = null;
            foreach (DataRow row in offerData.Rows)
            {
                int homeID = (int)row["HomeID"];
                string apiUrl = $"https://cis-iis2.temple.edu/Fall2024/CIS3342_tui78495/WebAPI/ReadHome/ReadSingleHomeListing/{homeID}";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                string jsonString = response.Content.ReadAsStringAsync().Result;
                Home rowHome = JsonConvert.DeserializeObject<Home>(jsonString);

                Client rowClient = ReadClients.GetClientByClientID((int)row["ClientID"]);

                if ((int)row["Amount"] == amount && rowHome.HomeID == home.HomeID && rowClient.ClientID == offerClient.ClientID)
                {
                    selectedOffer = new Offer((int)row["OfferID"], rowHome, ReadClients.GetClientByClientID((int)row["ClientID"]), (int)row["Amount"], (TypeOfSale)Enum.Parse(typeof(TypeOfSale), (string)row["TypeOfSale"]), (bool)row["SellHomePrior"], (DateTime)row["MoveInDate"], (OfferStatus)Enum.Parse(typeof(OfferStatus), (string)row["OfferStatus"]));
                }
            }


            return selectedOffer;
        }
    }
}
