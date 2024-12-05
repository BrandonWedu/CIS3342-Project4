

namespace HomeListingAPI
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
    [Serializable]
    public class Home : ICloneable<Home>
    {
        private int? homeID;
        private int agentID;
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

        public Home(int agentID, int cost, Address address, PropertyType type, int yearConstructed, GarageType garageType, string description, DateTime dateListed, SaleStatus saleStatus, Images images, Amenities amenities, TemperatureControl temperatureControl, Rooms rooms, Utilities utilities)
        {
            this.homeID = null;
            this.agentID = agentID;
            this.cost = cost;
            this.address = address.Clone();
            this.propertyType = type;
            this.yearConstructed = yearConstructed;
            this.garageType = garageType;
            this.description = description;
            this.dateListed = new DateTime(dateListed.Ticks);
            this.saleStatus = saleStatus;
            this.images = images.Clone();
            this.amenities = amenities.Clone();
            this.temperatureControl = temperatureControl.Clone();
            this.rooms = rooms.Clone();
            this.utilities = utilities.Clone();
        }
        public Home(int? houseID, int agent, int cost, Address address, PropertyType type, int yearConstructed, GarageType garageType, string description, DateTime dateListed, SaleStatus saleStatus, Images images, Amenities amenities, TemperatureControl temperatureControl, Rooms rooms, Utilities utilities)
        {
            this.homeID = houseID;
            this.agentID = agent;
            this.cost = cost;
            this.address = address.Clone();
            this.propertyType = type;
            this.yearConstructed = yearConstructed;
            this.garageType = garageType;
            this.description = description;
            this.dateListed = new DateTime(dateListed.Ticks);
            this.saleStatus = saleStatus;
            this.images = images.Clone();
            this.amenities = amenities.Clone();
            this.temperatureControl = temperatureControl.Clone();
            this.rooms = rooms.Clone();
            this.utilities = utilities.Clone();
        }

        public Home()
        {

        }

        public int? HomeID
        {
            get { return homeID; }
            set { homeID = value; }
        }
        public int AgentID
        {
            get { return agentID; }
            set { agentID = value; }
        }
        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        public Address Address
        {
            get { return address.Clone(); }
            set { address = value.Clone(); }
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
            get { return images.Clone(); }
            set { images = value.Clone(); }
        }
        public Amenities Amenities
        {
            get { return amenities.Clone(); }
            set { amenities = value.Clone(); }
        }
        public TemperatureControl TemperatureControl
        {
            get { return temperatureControl.Clone(); }
            set { temperatureControl = value.Clone(); }
        }
        public Rooms Rooms
        {
            get { return rooms.Clone(); }
            set { rooms = value.Clone(); }
        }
        public Utilities Utilities
        {
            get { return utilities.Clone(); }
            set { utilities = value.Clone(); }
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
        public Home Clone()
        {
            return new Home(HomeID, AgentID, Cost, Address, PropertyType, YearConstructed, GarageType, Description, DateListed, SaleStatus, Images, Amenities, TemperatureControl, Rooms, Utilities);
        }
    }
}
