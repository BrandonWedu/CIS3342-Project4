namespace Project4.Models
{
	public class Offers : ListOfObjects<Offer>
	{
		public Offers() { }

		public Offers(List<Offer> list) { List = list; }

		public Offers Clone()
		{
			return new Offers(List);
		}
	}
}
