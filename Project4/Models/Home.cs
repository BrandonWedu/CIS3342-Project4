using Newtonsoft.Json;

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
        public Home(int? houseID, int agentID, int cost, Address address, PropertyType type, int yearConstructed, GarageType garageType, string description, DateTime dateListed, SaleStatus saleStatus, Images images, Amenities amenities, TemperatureControl temperatureControl, Rooms rooms, Utilities utilities)
        {
            this.homeID = houseID;
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

        public Home()
        {

        }
		[JsonProperty("homeID")]
		public int? HomeID
        {
            get { return homeID; }
            set { homeID = value; }
        }
		[JsonProperty("agentID")]
		public int AgentID
        {
            get { return agentID; }
            set { agentID = value; }
        }
		[JsonProperty("cost")]
		public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }
		[JsonProperty("address")]
		public Address Address
        {
			get { return address != null ? address.Clone() : null; }
			set { address = value != null ? value.Clone() : null; }
		}
		[JsonProperty("propertyType")]
		public PropertyType PropertyType
        {
            get { return propertyType; }
            set { propertyType = value; }
        }
		[JsonProperty("homeSize")]
		public int HomeSize
        {
            get { return CalculateHomeSize(); }
        }
		[JsonProperty("yearConstructed")]
		public int YearConstructed
        {
            get { return yearConstructed; }
            set { yearConstructed = value; }
        }
		[JsonProperty("garageType")]
		public GarageType GarageType
        {
            get { return garageType; }
            set { garageType = value; }
        }
		[JsonProperty("description")]
		public string Description
        {
            get { return description; }
            set { description = value; }
        }
		[JsonProperty("dateListed")]
		public DateTime DateListed
        {
            get { return new DateTime(dateListed.Ticks); }
            set { dateListed = new DateTime(value.Ticks); }
        }
		[JsonProperty("saleStatus")]
		public SaleStatus SaleStatus
        {
            get { return saleStatus; }
            set { saleStatus = value; }
        }
		[JsonProperty("images")]
		public Images Images
        {
			get { return images ??= new Images(); } 
			set { images = value; } 
		}
		[JsonProperty("amenities")]
		public Amenities Amenities
        {
			get { return amenities ??= new Amenities(); }
			set { amenities = value; }
		}
		[JsonProperty("temperatureControl")]
		public TemperatureControl TemperatureControl
        {
			get { return temperatureControl ??= new TemperatureControl(); }
			set { temperatureControl = value; }
		}
		[JsonProperty("rooms")]
		public Rooms Rooms
        {
			get { return rooms ??= new Rooms(); }
			set { rooms = value; }
		}
		[JsonProperty("utilities")]
		public Utilities Utilities
        {
			get { return utilities ??= new Utilities(); }
			set { utilities = value; }
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
            return new Home(HomeID, AgentID, Cost, Address, PropertyType,YearConstructed, GarageType, Description, DateListed, SaleStatus, Images, Amenities, TemperatureControl, Rooms, Utilities);
        }
    }
}
