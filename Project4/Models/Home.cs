namespace Project4.Models
{
    //Property Type Enum
    public enum PropertyType
    {
        Townhome,
        Multifamily,
        Condo,
        Duplex,
        Tinyhome,
        SingleFamily
    }
    //House status Enum
    public enum SaleStatus
    {
        OffMarket,
        ForSale,
        Sold
    }
    //enum for GarageTypes
    public enum GarageType
    {
        SingleCar,
        DoubleCar,
        MultiCar
    }
    public class Home
    {
        //Fields
        private int? homeID;
        private Agent agent;
        private int cost;
        private Address address;
        private PropertyType propertyType;
        private int homeSize;
        private int yearConstructed;
        private GarageType garageType;
        private string description;
        private DateTime dateListed;
        private SaleStatus saleStatus;
        private Images images;
        private Amenities amenities;
        private TemperatureControl temperatureControl;
        private Rooms rooms;
        private Utilities utilities;

        //Constructor without id
        public Home(Agent agent, int cost, Address address, PropertyType type, int yearConstructed, GarageType garageType, string description, DateTime dateListed, SaleStatus saleStatus, Images images, Amenities amenities, TemperatureControl temperatureControl, Rooms rooms, Utilities utilities)
        {
            this.homeID = null;
            this.agent = agent.DeepCopy();
            this.cost = cost;
            this.address = address.DeepCopy();
            this.propertyType = type;
            this.yearConstructed = yearConstructed;
            this.garageType = garageType;
            this.description = description;
            this.dateListed = new DateTime(dateListed.Ticks);
            this.saleStatus = saleStatus;
            this.images = images.DeepCopy();
            this.amenities = amenities.DeepCopy();
            this.temperatureControl = temperatureControl.DeepCopy();
            this.rooms = rooms.DeepCopy();
            this.utilities = utilities.DeepCopy();
        }
        //Constructor with id
        public Home(int? houseID, Agent agent, int cost, Address address, PropertyType type, int yearConstructed, GarageType garageType, string description, DateTime dateListed, SaleStatus saleStatus, Images images, Amenities amenities, TemperatureControl temperatureControl, Rooms rooms, Utilities utilities)
        {
            this.homeID = houseID;
            this.agent = agent.DeepCopy();
            this.cost = cost;
            this.address = address.DeepCopy();
            this.propertyType = type;
            this.yearConstructed = yearConstructed;
            this.garageType = garageType;
            this.description = description;
            this.dateListed = new DateTime(dateListed.Ticks);
            this.saleStatus = saleStatus;
            this.images = images.DeepCopy();
            this.amenities = amenities.DeepCopy();
            this.temperatureControl = temperatureControl.DeepCopy();
            this.rooms = rooms.DeepCopy();
            this.utilities = utilities.DeepCopy();
        }

        //Get Set
        public int? HomeID
        {
            get { return homeID; }
            set { homeID = value; }
        }
        public Agent Agent
        {
            get { return agent.DeepCopy(); }
            set { agent = value.DeepCopy(); }
        }
        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        public Address Address
        {
            get { return address.DeepCopy(); }
            set { address = value.DeepCopy(); }
        }
        public PropertyType PropertyType
        {
            get { return propertyType; }
            set { propertyType = value; }
        }
        public int HomeSize
        {
            get { return CalculateHomeSize(); }
        }
        public int YearConstructed
        {
            get { return yearConstructed; }
            set { yearConstructed = value; }
        }
        public GarageType GarageType
        {
            get { return garageType; }
            set { garageType = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public DateTime DateListed
        {
            get { return new DateTime(dateListed.Ticks); }
            set { dateListed = new DateTime(value.Ticks); }
        }
        public SaleStatus SaleStatus
        {
            get { return saleStatus; }
            set { saleStatus = value; }
        }
        public Images Images
        {
            get { return images.DeepCopy(); }
            set { images = value.DeepCopy(); }
        }
        public Amenities Amenities
        {
            get { return amenities.DeepCopy(); }
            set { amenities = value.DeepCopy(); }
        }
        public TemperatureControl TemperatureControl
        {
            get { return temperatureControl.DeepCopy(); }
            set { temperatureControl = value.DeepCopy(); }
        }
        public Rooms Rooms
        {
            get { return rooms.DeepCopy(); }
            set { rooms = value.DeepCopy(); }
        }
        public Utilities Utilities
        {
            get { return utilities.DeepCopy(); }
            set { utilities = value.DeepCopy(); }
        }

        //Calculate home size
        private int CalculateHomeSize()
        {
            homeSize = 0;
            foreach (Room room in rooms.List)
            {
                homeSize += room.Width * room.Height;
            }
            return homeSize;
        }
        //Calculate time on market
        public int TimeOnMarket()
        {
            return (DateListed - DateTime.Now).Days;
        }
    }
}
