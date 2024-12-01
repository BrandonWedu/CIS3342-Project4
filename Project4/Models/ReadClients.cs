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
			sqlCommand.CommandText = "P4_SelectAllClients";
			DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentContactData.Rows)
			{
				allClients.Add(new Client((int)row["ClientID"], row["FirstName"].ToString(), row["LastName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["ClientAddress"]), row["PhoneNumber"].ToString(), row["Email"].ToString()));
			}

			return allClients;
		}

		internal static Client GetClientByClientID(int id)
		{
            DBConnect databaseHandler = new DBConnect();
            Clients allClients = new Clients();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllClients";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];
			Client selectedClient = null;
            foreach (DataRow row in agentContactData.Rows)
            {
				if ((int)row["ClientID"] == id)
				{
                    selectedClient = new Client((int)row["ClientID"], row["FirstName"].ToString(), row["LastName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["ClientAddress"]), row["PhoneNumber"].ToString(), row["Email"].ToString());

                }
            }

			return selectedClient;

		}

        internal static Client GetClientByLastNameAndAddress(Client client)
        {
            DBConnect databaseHandler = new DBConnect();
            Clients allClients = new Clients();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAllClients";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];
            Client selectedClient = null;
            foreach (DataRow row in agentContactData.Rows)
            {
                Address rowAddress = Serializer.DeserializeData<Address>((byte[])row["ClientAddress"]);
                if ((row["LastName"].ToString() == client.LastName) && (rowAddress.Street == client.ClientAddress.Street))
                {
                    selectedClient = new Client((int)row["ClientID"], row["FirstName"].ToString(), row["LastName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["ClientAddress"]), row["PhoneNumber"].ToString(), row["Email"].ToString());

                }
            }

            return selectedClient;

        }
    }
}
