namespace Project4.Models
{
	public class AgentList
	{
		private List<Agent> allAgents = new List<Agent>();
		private PasswordHasher passwordHasher = new PasswordHasher();

		public AgentList()
		{

		}

		public List<Agent> GetAllAgents()
		{
			return allAgents;
		}

		public Agent GetAgentByUsername(string username)
		{
			List<Agent> currentAgent = new List<Agent>() ;
			foreach (Agent agent in allAgents)
			{
				if (agent.AgentUsername == username)
				{
					currentAgent.Add(agent);
				}
			}
			return currentAgent[0];
		}

		public void CreateNewAgent()
		{

		}

	}
}
