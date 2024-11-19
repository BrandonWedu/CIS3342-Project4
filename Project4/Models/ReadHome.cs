using Microsoft.Data.SqlClient;
using static Azure.Core.HttpHeader;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace Project4.Models
{
    internal static class ReadHome
    {
        internal static Home GetHomeByID(int homeID)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_GetHomeByID";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", homeID, SqlDbType.Int, 8));

            DataSet dataSet = dbConnect.GetDataSet(sqlCommand);
            DataRow row = dataSet.Tables[0].Rows[0];
            return new Home
                (
                    homeID,
                    ReadAgent.GetAgentByID((int)row["AgentID"]),
                    (int)row["Cost"],
                    Serializer.DeserializeData<Address>((byte[])row["HomeAddress"]),
                    (PropertyType)Enum.Parse(typeof(PropertyType), (string)row["PropertyType"]),
                    (int)row["ConstructionYear"],
                    (GarageType)Enum.Parse(typeof(GarageType), (string)row["Garage"]),
                    (string)row["HomeDescription"],
                    (DateTime)row["DateListed"],
                    (SaleStatus)Enum.Parse(typeof(SaleStatus), (string)row["SaleStatus"]),
                    ReadImages.GetByHomeID(homeID),
                    ReadAmenities.GetByHomeID(homeID),
                    ReadTemperatureControl.GetByHomeID(homeID),
                    ReadRooms.GetByHomeID(homeID),
                    ReadUtilities.GetByHomeID(homeID)
                );
        }
    }
}
