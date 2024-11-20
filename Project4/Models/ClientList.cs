namespace Project4.Models
{
	public class ClientList : ListOfObjects<Client>
	{
		public ClientList() { }

		public ClientList(List<Client> list) { List = list; }

		public ClientList Clone()
		{
			return new ClientList(List);
		}
	}
}
