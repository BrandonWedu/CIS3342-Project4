using System.Collections.Generic;

namespace Project4.Models
{
	public class CompanyList : ListOfObjects<Company>
	{
		public CompanyList() { }

		public CompanyList(List<Company> list) { List = list; }

		public CompanyList Clone()
		{
			return new CompanyList(List);
		}
	}
}
