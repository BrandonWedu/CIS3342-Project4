using System.Collections.Generic;

namespace HomeListingAPI
{
	public class ContingencyList : ListOfObjects<Contingency>
	{
		public ContingencyList() { }

		public ContingencyList(List<Contingency> list) { List = list; }

		public ContingencyList Clone()
		{
			return new ContingencyList(List);
		}
	}
}
