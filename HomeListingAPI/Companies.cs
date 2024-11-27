using System.Collections.Generic;

namespace HomeListingAPI
{
	public class Companies : ListOfObjects<Company>
	{
		public Companies() { }

		public Companies(List<Company> list) { List = list; }

		public Companies Clone()
		{
			return new Companies(List);
		}
	}
}
