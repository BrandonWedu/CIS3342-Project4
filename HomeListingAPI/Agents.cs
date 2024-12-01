using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
	[Serializable]
	public class Agents : ListOfObjects<Agent>
	{
		public Agents() { }

		public Agents(List<Agent> list) { List = list; }

		public Agents Clone()
		{
			return new Agents(List);
		}

	}
}
