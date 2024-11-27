using System.Collections.Generic;

namespace HomeListingAPI
{
	public class AgentPersonalInformations : ListOfObjects<AgentPersonalInformation>
	{
		public AgentPersonalInformations() { }

		public AgentPersonalInformations(List<AgentPersonalInformation> list) { List = list; }

		public AgentPersonalInformations Clone()
		{
			return new AgentPersonalInformations(List);
		}
	}
}
