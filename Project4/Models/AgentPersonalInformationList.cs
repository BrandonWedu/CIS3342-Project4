using System.Collections.Generic;

namespace Project4.Models
{
	public class AgentPersonalInformationList : ListOfObjects<AgentPersonalInformation>
	{
		public AgentPersonalInformationList() { }

		public AgentPersonalInformationList(List<AgentPersonalInformation> list) { List = list; }

		public AgentPersonalInformationList Clone()
		{
			return new AgentPersonalInformationList(List);
		}
	}
}
