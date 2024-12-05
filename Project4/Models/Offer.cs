namespace Project4.Models
{
    public enum OfferStatus
    {
        Pending,
        Rejected,
        Accepted
    }

    public enum TypeOfSale
    {
        Cash,
        ConventionalMortgage
    }
    public class Offer : ICloneable<Offer>
    {
        private int offerID;
        private Home home;
        private Client client;
        private int amount;
        private TypeOfSale typeOfSale;
        private bool sellHomePrior;
        private DateTime moveInDate;
        private OfferStatus status;

        public int OfferID
        {
            get { return offerID; }
            set { offerID = value; }
        }

        public Home Home
        {
            get { return home; }
            set { home = value; }
        }

        public Client Client
        {
            get { return client; }
            set { client = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public TypeOfSale TypeOfSale
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

        public Offer(int id, Home home, Client client, int offerAmount, TypeOfSale saleType, bool sellHome, DateTime moveInDate, OfferStatus status)
        {
            OfferID = id;
            Home = home;
            Client = client;
            Amount = offerAmount;
            TypeOfSale = saleType;
            SellHomePrior = sellHome;
            MoveInDate = moveInDate;
            Status = status;
        }

        public Offer(Home home, Client client, int offerAmount, TypeOfSale saleType, bool sellHome, DateTime moveInDate, OfferStatus status)
        {
            Home = home;
            Client = client;
            Amount = offerAmount;
            TypeOfSale = saleType;
            SellHomePrior = sellHome;
            MoveInDate = moveInDate;
            Status = status;
        }

        public Offer Clone()
        {
            return new Offer(OfferID, Home, Client, Amount, TypeOfSale, SellHomePrior, MoveInDate, Status);
        }
    }
}
