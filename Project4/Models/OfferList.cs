namespace Project4.Models
{
	public class OfferList : ListOfObjects<Offer>
	{
		public OfferList() { }

		public OfferList(List<Offer> list) { List = list; }

		public OfferList Clone()
		{
			return new OfferList(List);
		}
	}
}
