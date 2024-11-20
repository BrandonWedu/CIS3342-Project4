namespace Project4.Models
{
	public class Company : ICloneable<Company>
	{
		private int companyID;
		private string companyName;
		private Address companyAddress;
		private string phoneNumber;
		private string email;

		public int CompanyID
		{
			get { return companyID; }
			set { companyID = value; }
		}
		public string CompanyName
		{
			get { return companyName; }
			set { companyName = value; }
		}

		public Address CompanyAddress
		{
			get { return companyAddress; }
			set { companyAddress = value; }
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

		public Company(int id, string name, Address address, string phone, string email)
		{
			companyID = id;
			companyName = name;
			companyAddress = address;
			phoneNumber = phone;
			Email = email;
		}

		public Company Clone()
		{
			return new Company(CompanyID, CompanyName, CompanyAddress, PhoneNumber, Email);
		}
	}
}
