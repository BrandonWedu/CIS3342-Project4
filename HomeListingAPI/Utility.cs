namespace HomeListingAPI
{
    public enum UtilityTypes
    {
        Electricity,
        NaturalGas,
        WellWater,
        PublicSupply,
        PublicSewer,
        Septic,
        Internet,
        CableTelevision,
        TrashCollection,
        Telephone,
        WasteManagement,
        StormwaterManagement
    }
	[Serializable]
	public class Utility : ICloneable<Utility>
    {
        private int? utilityID;
        private UtilityTypes type;
        private string information;

        public Utility(UtilityTypes type, string information)
        {
            utilityID = null;
            this.type = type;
            this.information = information;
        }
        public Utility(int? utilityID, UtilityTypes type, string information)
        {
            this.utilityID = utilityID;
            this.type = type;
            this.information = information;
        }

        public Utility()
        {

        }

        public int? UtilityID
        {
            get { return utilityID; }
            set { utilityID = value; }
        }
        public UtilityTypes Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Information
        {
            get { return information; }
            set { information = value; }
        }

        public Utility Clone()
        {
            return new Utility(UtilityID, Type, Information);
        }
    }
}
