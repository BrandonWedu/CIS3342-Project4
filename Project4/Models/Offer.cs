namespace Project4.Models
{
	public enum OfferStatus
	{
		Pending,
		Rejected,
		Accepted
	}
	public class Offer : ICloneable<Offer>
	{
		private int offerID;
		private int homeID;
		private int clientID;
		private decimal amount;
		private string typeOfSale;
		private bool sellHomePrior;
		private DateTime moveInDate;
		private OfferStatus status;

		public int OfferID
		{
			get { return offerID; }
			set { offerID = value; }
		}

		public int HomeID
		{
			get { return homeID; }
			set { homeID = value; }
		}

		public int ClientID
		{
			get { return clientID; }
			set { clientID = value; }
		}

		public decimal Amount
		{
			get { return amount; }
			set { amount = value; }
		}

		public string TypeOfSale
		{
			get { return typeOfSale; }
			set { typeOfSale = value; }
		}

		public bool SellHomePrior
		{
			get { return sellHomePrior; }
			set { sellHomePrior = value; }
		}

		public DateTime MoveInDate
		{
			get { return moveInDate; }
			set { moveInDate = value; }
		}

		public OfferStatus Status
		{
			get { return status; }
			set { status = value; }
		}

		public Offer(int id, int homeID, int clientID, decimal offerAmount, string saleType, bool sellHome, DateTime moveInDate, OfferStatus status)
		{
			OfferID = id;
			HomeID = homeID;
			ClientID = clientID;
			Amount = offerAmount;
			TypeOfSale = saleType;
			SellHomePrior = sellHome;
			MoveInDate = moveInDate;
			Status = status;
		}

		public Offer Clone()
		{
			return new Offer(OfferID, HomeID, ClientID, Amount, TypeOfSale, SellHomePrior, MoveInDate, Status);
		}
	}
}
