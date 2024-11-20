﻿using System.Reflection;

namespace Project4.Models
{
	public class Agent : ICloneable<Agent>
	{
		private int agentID;
		private string agentUsername;
		private string agentPassword;
		private string agentPasswordSalt;
		private Company workCompany;
		private AgentContact agentContactInfo;
		private AgentPersonalInformation personalInformation;
		private AgentSecurityList agentSecurityQuestions;

		public int AgentID
		{
			get { return agentID; }
			set { agentID = value; }
		}

		public string AgentUsername
		{
			get { return agentUsername; }
			set { agentUsername = value; }
		}
		public string AgentPassword
		{
			get { return agentPassword; }
			set { agentPassword = value; }
		}

		public string AgentPasswordSalt
		{
			get { return agentPasswordSalt; }
			set { agentPasswordSalt = value; }
		}

		public Company WorkCompany
		{
			get { return workCompany; }
			set { workCompany = value; }
		}

		public AgentContact AgentContactInfo
		{
			get { return agentContactInfo; }
			set { agentContactInfo = value; }
		}

		public AgentPersonalInformation PersonalInformation
		{
			get { return personalInformation; }
			set { personalInformation = value; }
		}

		public AgentSecurityList AgentSecurityQuestions
		{
			get { return agentSecurityQuestions; }
			set { agentSecurityQuestions = value; }
		}

		public Agent(int id, string username, string password, string salt, Company work, AgentContact contactInfo, AgentPersonalInformation agentInfo, AgentSecurityList questions)
		{
			AgentID = id;
			AgentUsername = username;
			AgentPassword = password;
			AgentPasswordSalt = salt;
			WorkCompany = work;
			AgentContactInfo = contactInfo;
			PersonalInformation = agentInfo;
			AgentSecurityQuestions = questions;
		}


		public Agent Clone()
		{
			return new Agent(AgentID, AgentUsername, AgentPassword, AgentPasswordSalt, WorkCompany, AgentContactInfo, PersonalInformation, AgentSecurityQuestions);
		}


	}
}
