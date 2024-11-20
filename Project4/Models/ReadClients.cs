using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class ReadClients
	{
		private DBConnect databaseHandler = new DBConnect();
		private ClientList allClients = new ClientList();

		public ReadClients()
		{
			allClients = new ClientList();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "SelectAllCompanies";
			DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentContactData.Rows)
			{
				allClients.Add(new Client((int)row["ClientID"], row["FirstName"].ToString(), row["LastName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["ClientAddress"]), row["PhoneNumber"].ToString(), row["Email"].ToString()));
			}
		}

		public ClientList GetAllClients()
		{
			return allClients;
		}

		public ClientList GetClientByClientID(int id)
		{
			ClientList selectedClients = new ClientList();
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
