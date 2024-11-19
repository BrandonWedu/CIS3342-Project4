using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class AgentList : ListOfObjects<Agent>
	{
		public AgentList() { }

		public AgentList(List<Agent> list) { List = list; }

		public AgentList Clone()
		{
			return new AgentList(List);
		}

	}
}
