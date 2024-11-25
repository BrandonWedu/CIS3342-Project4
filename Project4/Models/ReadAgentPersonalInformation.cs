using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public class ReadAgentPersonalInformation
    {
        private DBConnect databaseHandler = new DBConnect();


        internal static AgentPersonalInformations ReadAllAgentPeronsalInformation()
        {
            DBConnect databaseHandler = new DBConnect();
            AgentPersonalInformations allAgentPersonalInfo = new AgentPersonalInformations();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAgentPersonalInformation";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allAgentPersonalInfo.Add(new AgentPersonalInformation((int)row["AgentInfoID"], (int)row["AgentID"], row["FirstName"].ToString(), row["LastName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["PersonalAddress"]), row["PersonalPhoneNumber"].ToString(), row["PersonalEmail"].ToString()));
            }

            return allAgentPersonalInfo;
        }


        internal static AgentPersonalInformations GetAgentPeronsalInformationByAgentID(int id)
        {
            DBConnect databaseHandler = new DBConnect();
            AgentPersonalInformations allAgentPersonalInfo = new AgentPersonalInformations();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectAgentPersonalInformation";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allAgentPersonalInfo.Add(new AgentPersonalInformation((int)row["AgentInfoID"], (int)row["AgentID"], row["FirstName"].ToString(), row["LastName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["PersonalAddress"]), row["PersonalPhoneNumber"].ToString(), row["PersonalEmail"].ToString()));
            }
            AgentPersonalInformations selectedAgent = new AgentPersonalInformations();
            foreach (AgentPersonalInformation currentInfo in allAgentPersonalInfo.List)
            {
                if (currentInfo.AgentID == id)
                {
                    selectedAgent.Add(currentInfo);
                }
            }

            return selectedAgent;
        }
    }
}
