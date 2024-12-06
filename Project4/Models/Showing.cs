namespace Project4.Models
{
    //Showing Status
    public enum ShowingStatus
    {
        Pending,
        Rejected,
        Accepted
    }
    public class Showing : ICloneable<Showing>
    {
        //Fields
        private int? showingID;
        private int homeID;
        private Client client;
        private DateTime timeRequestCreated;
        private DateTime showingTime;
        private ShowingStatus status;

        public Showing(int homeID, Client client, DateTime timeRequestCreated, DateTime showingTime, ShowingStatus status)
        {
            showingID = null;
            this.homeID = homeID;
            this.client = client.Clone();
            this.timeRequestCreated = new DateTime(timeRequestCreated.Ticks);
            this.showingTime = new DateTime(showingTime.Ticks);
            this.status = status;
        }

        public Showing(int? showingID, int homeID, Client client, DateTime timeRequestCreated, DateTime showingTime, ShowingStatus status)
        {
            this.showingID = showingID;
            this.homeID = homeID;
            this.client = client.Clone();
            this.timeRequestCreated = new DateTime(timeRequestCreated.Ticks);
            this.showingTime = new DateTime(showingTime.Ticks);
            this.status = status;
        }

        public int? ShowingID
        {
            get { return showingID; }
            set { showingID = value; }
        }
        public int HomeID
        {
            get { return homeID; }
            set { homeID = value; }
        }
        public Client Client
        {
            get { return client.Clone(); }
            set { client = value.Clone(); }
        }
        public DateTime TimeRequestCreated
        {
            get { return new DateTime(timeRequestCreated.Ticks); }
            set { timeRequestCreated = new DateTime(value.Ticks); }
        }
        public DateTime ShowingTime
        {
            get { return new DateTime(showingTime.Ticks); }
            set { showingTime = new DateTime(value.Ticks); }
        }
        public ShowingStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public Showing Clone()
        {
            return new Showing(ShowingID, HomeID, Client, TimeRequestCreated, ShowingTime, Status);
        }
    }
}
