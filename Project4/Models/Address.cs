namespace Project4.Models
{
    //Contains data for an Address
    public class Address
    {
        private string street;
        private string city;
        private string state;
        private string zipCode;

        public Address(string street, string city, string state, string zipCode)
        {
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
        public string State
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
    }
}
