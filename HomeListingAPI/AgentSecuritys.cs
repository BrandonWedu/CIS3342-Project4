using System.Collections.Generic;

namespace HomeListingAPI
{
	public class AgentSecuritys : ListOfObjects<AgentSecurity>
	{
		public AgentSecuritys() { }

		public AgentSecuritys(List<AgentSecurity> list) { List = list; }

		public AgentSecuritys Clone()
		{
			return new AgentSecuritys(List);
		}
	}
}
