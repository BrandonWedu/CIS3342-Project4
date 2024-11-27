using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
    public class ReadAgentContact
    {
        internal static AgentContacts ReadAllAgentContacts()
        {

            DBConnect databaseHandler = new DBConnect();
            AgentContacts allAgentContact = new AgentContacts();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAgentContacts";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allAgentContact.Add(new AgentContact((int)row["AgentContactID"], (int)row["AgentID"], Serializer.DeserializeData<Address>((byte[])row["WorkAddress"]), row["WorkPhoneNumber"].ToString(), row["WorkEmail"].ToString()));
            }

            return allAgentContact;

        }

        internal static AgentContacts GetAgentContactByAgentID(int agentID)
        {
            DBConnect databaseHandler = new DBConnect();
            AgentContacts allAgentContact = new AgentContacts();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAgentContacts";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allAgentContact.Add(new AgentContact((int)row["AgentContactID"], (int)row["AgentID"], Serializer.DeserializeData<Address>((byte[])row["WorkAddress"]), row["WorkPhoneNumber"].ToString(), row["WorkEmail"].ToString()));
            }
            AgentContacts agentContact = new AgentContacts();

            foreach (AgentContact contactInfo in allAgentContact.List)
            {
                if (contactInfo.AgentID == agentID)
                {
                    agentContact.Add(contactInfo);
                }
            }
            return agentContact;
        }
    }
}
