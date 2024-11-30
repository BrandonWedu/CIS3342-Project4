namespace Project4.Models.ViewModels
{
	public class MakeOfferViewModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string SaleType { get; set; }

		public string OfferAmount { get; set; }

		public string NewContingency {  get; set; }

		public bool SellCurrentHome {  get; set; }

		public DateTime MoveInDate { get; set; }

		public Contingencies offerContingencies;
	}
}
