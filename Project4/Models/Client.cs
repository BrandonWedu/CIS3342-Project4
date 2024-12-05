namespace Project4.Models
{
    public class Client : ICloneable<Client>
    {
        private int clientID;
        private string firstName;
        private string lastName;
        private Address clientAddress;
        private string phoneNumber;
        private string email;

        public int ClientID
        {
            get { return clientID; }
            set { clientID = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public Address ClientAddress
        {
            get { return clientAddress; }
            set { clientAddress = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public Client(int id, string fName, string lName, Address address, string phone, string email)
        {
            ClientID = id;
            FirstName = fName;
            LastName = lName;
            ClientAddress = address;
            PhoneNumber = phone;
            Email = email;
        }

        public Client(string fName, string lName, Address address, string phone, string email)
        {
            FirstName = fName;
            LastName = lName;
            ClientAddress = address;
            PhoneNumber = phone;
            Email = email;
        }

        public Client Clone()
        {
            return new Client(ClientID, FirstName, LastName, ClientAddress, PhoneNumber, Email);
        }

    }
}
