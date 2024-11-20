namespace Project4.Models
{
	public class Contingency : ICloneable<Contingency>
	{
		private int contingencyID;
		private int offerID;
		private string contingecny;

		public int ContingencyID
		{
			get { return contingencyID; }
			set { contingencyID = value; }
		}

		public int OfferID
		{
			get { return offerID; }
			set { offerID = value; }
		}

		public string Contingecny
		{
			get { return contingecny; }
			set { contingecny = value; }
		}

		public Contingency(int id, int offerID, string contingency)
		{
			ContingencyID = id;
			OfferID = offerID;
			Contingecny = contingency;
		}

		public Contingency Clone()
		{
			return new Contingency(ContingencyID, OfferID, Contingecny);
		}

	}
}
