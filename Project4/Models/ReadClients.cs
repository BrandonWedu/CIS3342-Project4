using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class ReadClients
	{
		private DBConnect databaseHandler = new DBConnect();
		private Clients allClients = new Clients();

		internal static Clients ReadAllClients()
		{
            DBConnect databaseHandler = new DBConnect();
            Clients allClients = new Clients();
            SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "SelectAllCompanies";
			DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentContactData.Rows)
			{
				allClients.Add(new Client((int)row["ClientID"], row["FirstName"].ToString(), row["LastName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["ClientAddress"]), row["PhoneNumber"].ToString(), row["Email"].ToString()));
			}

			return allClients;
		}

		internal static Clients GetClientByClientID(int id)
		{
            DBConnect databaseHandler = new DBConnect();
            Clients allClients = new Clients();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectAllCompanies";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allClients.Add(new Client((int)row["ClientID"], row["FirstName"].ToString(), row["LastName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["ClientAddress"]), row["PhoneNumber"].ToString(), row["Email"].ToString()));
            }
            Clients selectedClients = new Clients();
			foreach (Client client in allClients.List)
			{
				if (client.ClientID == id)
				{
					selectedClients.Add(client);
				}
			}
			return selectedClients;
		}
	}
}
