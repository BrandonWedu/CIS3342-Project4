using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class ReadAgentSecurity
	{
		private DBConnect databaseHandler = new DBConnect();
		private AgentSecurityList allAgentSecurity = new AgentSecurityList();

		public ReadAgentSecurity()
		{
			allAgentSecurity = new AgentSecurityList();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "SelectAllAgentSecurity";
			DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentContactData.Rows)
			{
				allAgentSecurity.Add(new AgentSecurity((int)row["SecurityQuestionsID"], (int)row["AgentID"], row["Question"].ToString(), row["Answer"].ToString()));
			}
		}

		public AgentSecurityList GetAllAgentSecurityQuestion()
		{
			return allAgentSecurity;
		}

		public AgentSecurityList GetAgentSecurityQuestionsByAgentID(int id)
		{
			AgentSecurityList selectedQuestions = new AgentSecurityList();
			foreach (AgentSecurity currentSecurity in  allAgentSecurity.List)
			{
				if (currentSecurity.AgentID == id)
				{
					selectedQuestions.Add(currentSecurity);
				}
			}
			return selectedQuestions;
		}
	}
}
