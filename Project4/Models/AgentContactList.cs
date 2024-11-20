using System.Collections.Generic;

namespace Project4.Models
{
	public class AgentContactList : ListOfObjects<AgentContact>
	{
		public AgentContactList() { }

		public AgentContactList(List<AgentContact> list) { List = list; }

		public AgentContactList Clone()
		{
			return new AgentContactList(List);
		}
	}
}
