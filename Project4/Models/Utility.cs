using Newtonsoft.Json;

namespace Project4.Models
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

		[JsonProperty("utilityID")]
		public int? UtilityID
        {
            get { return utilityID; }
            set { utilityID = value; }
        }
		[JsonProperty("type")]
		public UtilityTypes Type
        {
            get { return type; }
            set { type = value; }
        }
		[JsonProperty("information")]
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
