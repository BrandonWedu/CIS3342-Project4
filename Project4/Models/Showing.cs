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
        private Home home;
        private Client client; 
        private DateTime timeRequestCreated;
        private DateTime showingTime;
        private ShowingStatus status;

        public Showing(Home home, Client client, DateTime timeRequestCreated, DateTime showingTime, ShowingStatus status)
        {
            showingID = null;
            this.home = home.Clone();
            this.client = client.Clone();
            this.timeRequestCreated = new DateTime(timeRequestCreated.Ticks);
            this.showingTime = new DateTime(showingTime.Ticks);
            this.status = status;
        }

        public Showing(int? showingID, Home home, Client client, DateTime timeRequestCreated, DateTime showingTime, ShowingStatus status)
        {
            this.showingID = showingID;
            this.home = home.Clone();
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
        public Home Home
        {
            get { return home.Clone(); }
            set { home = value.Clone(); }
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
            return new Showing(ShowingID, Home, Client, TimeRequestCreated, ShowingTime, Status);
        }
    }
}
