﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class ReadAgentSecurity
	{

		internal static AgentSecuritys ReadAllAgentSecurity()
		{
            DBConnect databaseHandler = new DBConnect();
            AgentSecuritys allAgentSecurity = new AgentSecuritys();
            SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "SelectAllAgentSecurity";
			DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentContactData.Rows)
			{
				allAgentSecurity.Add(new AgentSecurity((int)row["SecurityQuestionsID"], (int)row["AgentID"], row["Question"].ToString(), row["Answer"].ToString()));
			}

			return allAgentSecurity;
		}


		internal static AgentSecuritys GetAgentSecurityQuestionsByAgentID(int id)
		{
            DBConnect databaseHandler = new DBConnect();
            AgentSecuritys allAgentSecurity = new AgentSecuritys();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectAllAgentSecurity";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allAgentSecurity.Add(new AgentSecurity((int)row["SecurityQuestionsID"], (int)row["AgentID"], row["Question"].ToString(), row["Answer"].ToString()));
            }
            AgentSecuritys selectedQuestions = new AgentSecuritys();
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
