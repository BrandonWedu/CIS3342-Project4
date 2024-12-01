using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class WriteOffer
	{
        internal static int CreateNew(Offer offer)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewOffer";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@HomeID", (int)offer.Home.HomeID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@ClientID", offer.Client.ClientID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@Amount", (int)offer.Amount, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@TypeOfSale", offer.TypeOfSale.ToString(), SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<bool>("@SellHomePrior", offer.SellHomePrior, SqlDbType.Bit));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<DateTime>("@MoveInDate", offer.MoveInDate, SqlDbType.Date));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@Status", offer.Status.ToString(), SqlDbType.VarChar, 50));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@OfferID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);

            return (int)outputParam.Value;
        }


		internal static void UpdateOfferStatus(int offerID, OfferStatus newStatus)
		{
			DBConnect dbConnect = new DBConnect();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "P4_UpdateOfferStatus";

			sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@OfferID", offerID, SqlDbType.Int, 8));
			sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@Status", newStatus.ToString(), SqlDbType.VarChar, 50));
			dbConnect.DoUpdate(sqlCommand);
		}
	}
}
