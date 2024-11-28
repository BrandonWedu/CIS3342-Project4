namespace Project4.Models
{
    public enum States
    {
        Alabama,
        Alaska,
        Arizona,
        Arkansas,
        California,
        Colorado,
        Connecticut,
        Delaware,
        Florida,
        Georgia,
        Hawaii,
        Idaho,
        Illinois,
        Indiana,
        Iowa,
        Kansas,
        Kentucky,
        Louisiana,
        Maine,
        Maryland,
        Massachusetts,
        Michigan,
        Minnesota,
        Mississippi,
        Missouri,
        Montana,
        Nebraska,
        Nevada,
        NewHampshire,
        NewJersey,
        NewMexico,
        NewYork,
        NorthCarolina,
        NorthDakota,
        Ohio,
        Oklahoma,
        Oregon,
        Pennsylvania,
        RhodeIsland,
        SouthCarolina,
        SouthDakota,
        Tennessee,
        Texas,
        Utah,
        Vermont,
        Virginia,
        Washington,
        WestVirginia,
        Wisconsin,
        Wyoming
    }
    [Serializable]
    public class Address : ICloneable<Address>
    {
        private int? addressID;
        private string street;
        private string city;
        private States state;
        private string zipCode;

        public Address() { }
        public Address(string street, string city, States state, string zipCode)
        {
            addressID = null;
            this.street = street;
            this.city = city;
            this.state = state;
            this.zipCode = zipCode;
        }
        public Address(int? addressID, string street, string city, States state, string zipCode)
        {
            this.addressID = addressID;
            this.street = street;
            this.city = city;
            this.state = state;
            this.zipCode = zipCode;
        }

        public string Street
        {
            get { return street; }
            set { this.street = value; }
        }
        public string City
        {
            get { return city; }
            set { this.city = value; }
        }
        public States State
        {
            get { return state; }
            set { this.state = value; }
        }
        public string ZipCode
        {
            get { return zipCode; }
            set { this.zipCode = value; }
        }

        public override string ToString()
        {
            return $"{street}, {city}, {state} {ZipCode}";
        }
        public Address Clone()
        {
            return new Address(Street, City, State, ZipCode);
        }
    }
}
