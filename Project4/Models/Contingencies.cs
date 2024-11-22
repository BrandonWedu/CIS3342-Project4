using System.Collections.Generic;

namespace Project4.Models
{
	public class Contingencies : ListOfObjects<Contingency>
	{
		public Contingencies() { }

		public Contingencies(List<Contingency> list) { List = list; }

		public Contingencies Clone()
		{
			return new Contingencies(List);
		}
	}
}
