using System.Collections.Generic;

namespace Project4.Models
{
	public class AgentSecurityList : ListOfObjects<AgentSecurity>
	{
		public AgentSecurityList() { }

		public AgentSecurityList(List<AgentSecurity> list) { List = list; }

		public AgentSecurityList Clone()
		{
			return new AgentSecurityList(List);
		}
	}
}
