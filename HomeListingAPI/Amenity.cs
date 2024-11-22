namespace HomeListingAPI
{
    //emun for all amenity types
    public enum AmenityType
    {
        Fireplace,
        Dishwasher,
        WashingMachine,
        Dryer,
        Refrigerator,
        Microwave,
        BuiltinOven,
        SecuritySystem,
        Deck,
        Patio,
        SwimmingPool,
        HotTub,
        SmartHomeTechnology,
        CeilingFans,
        WalkinCloset,
        HardwoodFloors,
        GraniteCountertops,
        Garden
    }

    public class Amenity : ICloneable<Amenity>
    {
        private int? amenityID;
        private AmenityType type;
        private string description;

        public Amenity(AmenityType type, string description)
        {
            this.amenityID = null;
            this.type = type;
            this.description = description;
        }
        public Amenity(int? amenityID, AmenityType type, string description)
        {
            this.amenityID = amenityID;
            this.type = type;
            this.description = description;
        }

        public int? AmenityID
        {
            get { return amenityID; }
            set { amenityID = value; }
        }
        public AmenityType Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Amenity Clone()
        {
            return new Amenity(AmenityID, Type, Description);
        }
    }
}
