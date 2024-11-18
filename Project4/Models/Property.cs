namespace Project4.Models
{
    public class Property
    {
        private int propertyID; // private int variable called propertyID
        private int accountID; // private int variable called accountID
        private int addressZipCode; // private int variable called addressZipCode
        private string addressState; // private string variable called addressState
        private string addressCity; // private string variable called addressCity
        private string address; // private string variable called address
        private string propertyType; // private string variable called propertyType
        private decimal propertySize; // private decimal variable called propertySize
        private int numberOfBedrooms; // private int variable called numberOfBedrooms
        private decimal numberOfBathrooms; // private decimal variable called numberOfBathrooms
        private string garageType; // private string variable called garageType
        private string heatingSystem; // private string variable called heatingSystem
        private string coolingSystem; // private string variable called coolingSystem
        private int yearBuilt; // private int variable called yearBuilt
        private string utilitiesWater; // private string variable called utilitiesWater
        private string utilitiesSewer; // private string variable called utilitiesSewer
        private string propertyDesciption; // private string variable called propertyDesciption
        private decimal askingPrice; // private decimal variable called askingPrice
        private DateTime creationDate; // private DateTime object called statusID

        public int PropertyID // public int variable called PropertyID
        {
            get { return propertyID; } // returns the value of propertyID 
            set { propertyID = value; } // sets the value of propertyID variable to whatever is passed to it
        }

        public int AccountID // public int variable called AccountID
        {
            get { return accountID; } // returns the value of accountID 
            set { accountID = value; } // sets the value of accountID variable to whatever is passed to it
        }

        public int AddressZipCode // public int variable called AddressZipCode
        {
            get { return addressZipCode; } // returns the value of addressZipCode 
            set { addressZipCode = value; } // sets the value of addressZipCode variable to whatever is passed to it
        }

        public string AddressState // public string variable called AddressState
        {
            get { return addressState; } // returns the value of addressState 
            set { addressState = value; } // sets the value of addressState variable to whatever is passed to it
        }

        public string AddressCity // public string variable called AddressCity
        {
            get { return addressCity; } // returns the value of addressCity 
            set { addressCity = value; } // sets the value of addressCity variable to whatever is passed to it
        }

        public string AddressStreet // public string variable called AddressStreet
        {
            get { return address; } // returns the value of address 
            set { address = value; } // sets the value of address variable to whatever is passed to it
        }

        public string PropertyType // public string variable called PropertyType
        {
            get { return propertyType; } // returns the value of propertyType 
            set { propertyType = value; } // sets the value of propertyType variable to whatever is passed to it
        }

        public decimal PropertySize // public int variable called PropertySize
        {
            get { return propertySize; } // returns the value of propertySize 
            set { propertySize = value; } // sets the value of propertySize variable to whatever is passed to it
        }

        public int NumberOfBedrooms // public int variable called NumberOfBedrooms
        {
            get { return numberOfBedrooms; } // returns the value of numberOfBedrooms 
            set { numberOfBedrooms = value; } // sets the value of numberOfBedrooms variable to whatever is passed to it
        }

        public decimal NumberOfBathrooms // public int variable called NumberOfBathrooms
        {
            get { return numberOfBathrooms; } // returns the value of numberOfBathrooms 
            set { numberOfBathrooms = value; } // sets the value of numberOfBathrooms variable to whatever is passed to it
        }

        public string GarageType // public string variable called GarageType
        {
            get { return garageType; } // returns the value of garageType 
            set { garageType = value; } // sets the value of garageType variable to whatever is passed to it
        }

        public string HeatingSystem // public string variable called HeatingSystem
        {
            get { return heatingSystem; } // returns the value of heatingSystem 
            set { heatingSystem = value; } // sets the value of heatingSystem variable to whatever is passed to it
        }

        public string CoolingSystem // public string variable called CoolingSystem
        {
            get { return coolingSystem; } // returns the value of coolingSystem 
            set { coolingSystem = value; } // sets the value of coolingSystem variable to whatever is passed to it
        }

        public int YearBuilt // public int variable called YearBuilt
        {
            get { return yearBuilt; } // returns the value of yearBuilt 
            set { yearBuilt = value; } // sets the value of yearBuilt variable to whatever is passed to it
        }

        public string UtilitiesWater // public string variable called UtilitiesWater
        {
            get { return utilitiesWater; } // returns the value of utilitiesWater 
            set { utilitiesWater = value; } // sets the value of utilitiesWater variable to whatever is passed to it
        }

        public string UtilitiesSewer // public string variable called UtilitiesSewer
        {
            get { return utilitiesSewer; } // returns the value of utilitiesSewer 
            set { utilitiesSewer = value; } // sets the value of utilitiesSewer variable to whatever is passed to it
        }

        public string PropertyDescription // public string variable called PropertyDescription
        {
            get { return propertyDesciption; } // returns the value of propertyDesciption 
            set { propertyDesciption = value; } // sets the value of propertyDesciption variable to whatever is passed to it
        }

        public decimal AskingPrice // public int variable called AskingPrice
        {
            get { return askingPrice; } // returns the value of askingPrice 
            set { askingPrice = value; } // sets the value of askingPrice variable to whatever is passed to it
        }

        public DateTime CreationDate // public DateTime object called CreationDate
        {
            get { return creationDate; } // returns the value of creationDate 
            set { creationDate = value; } // sets the value of creationDate object to whatever is passed to it
        }

        public Property(int pID, int aID, int aZip, string aState, string aCity, string address, string type, decimal size, int bedrooms, decimal bathrooms, string garage, string heating, string cooling, int yBuilt, string water, string sewer, string description, decimal price) // public Property initalizer that accepts five ints, ten string, and three decimal parameters
        {
            PropertyID = pID; // sets PropertyID variable to the pID parameter
            AccountID = aID; // sets AccountID variable to the aID parameter
            AddressZipCode = aZip; // sets AddressZipCode variable to the aZip parameter
            AddressState = aState; // sets AddressState variable to the aState parameter
            AddressCity = aCity; // sets AddressCity variable to the aCity parameter
            AddressStreet = address; // sets AddressStreet variable to the address parameter
            PropertyType = type; // sets PropertyType variable to the type parameter
            PropertySize = size; // sets PropertySize variable to the size parameter
            NumberOfBedrooms = bedrooms; // sets NumberOfBedrooms variable to the bedrooms parameter
            NumberOfBathrooms = bathrooms; // sets NumberOfBathrooms variable to the bathrooms parameter
            GarageType = garage; // sets GarageType variable to the garage parameter
            HeatingSystem = heating; // sets HeatingSystem variable to the heating parameter
            CoolingSystem = cooling; // sets CoolingSystem variable to the cooling parameter
            YearBuilt = yBuilt; // sets YearBuilt variable to the yBuilt parameter
            UtilitiesWater = water; // sets UtilitiesWater variable to the water parameter
            UtilitiesSewer = sewer; // sets UtilitiesSewer variable to the sewer parameter
            PropertyDescription = description; // sets PropertyDescription variable to the description parameter
            AskingPrice = price; // sets AskingPrice variable to the price parameter
        }

        public Property(int pID, int aID, int aZip, string aState, string aCity, string address, string type, decimal size, int bedrooms, decimal bathrooms, string garage, string heating, string cooling, int yBuilt, string water, string sewer, string description, decimal price, DateTime creation) // public Property initalizer that accepts five ints, ten string, three decimal, and a DateTime parameters
        {
            PropertyID = pID; // sets PropertyID variable to the pID parameter
            AccountID = aID; // sets AccountID variable to the aID parameter
            AddressZipCode = aZip; // sets AddressZipCode variable to the aZip parameter
            AddressState = aState; // sets AddressState variable to the aState parameter
            AddressCity = aCity; // sets AddressCity variable to the aCity parameter
            AddressStreet = address; // sets AddressStreet variable to the address parameter
            PropertyType = type; // sets PropertyType variable to the type parameter
            PropertySize = size; // sets PropertySize variable to the size parameter
            NumberOfBedrooms = bedrooms; // sets NumberOfBedrooms variable to the bedrooms parameter
            NumberOfBathrooms = bathrooms; // sets NumberOfBathrooms variable to the bathrooms parameter
            GarageType = garage; // sets GarageType variable to the garage parameter
            HeatingSystem = heating; // sets HeatingSystem variable to the heating parameter
            CoolingSystem = cooling; // sets CoolingSystem variable to the cooling parameter
            YearBuilt = yBuilt; // sets YearBuilt variable to the yBuilt parameter
            UtilitiesWater = water; // sets UtilitiesWater variable to the water parameter
            UtilitiesSewer = sewer; // sets UtilitiesSewer variable to the sewer parameter
            PropertyDescription = description; // sets PropertyDescription variable to the description parameter
            AskingPrice = price; // sets AskingPrice variable to the price parameter
            CreationDate = creation; // sets CreationDate variable to the creation parameter
        }

        public Property(int aID, int aZip, string aState, string aCity, string address, string type, decimal size, int bedrooms, decimal bathrooms, string garage, string heating, string cooling, int yBuilt, string water, string sewer, string description, decimal price) // public Property initalizer that accepts four ints, ten strings, and three decimal parameters
        {
            AccountID = aID; // sets AccountID variable to the aID parameter
            AddressZipCode = aZip; // sets AddressZipCode variable to the aZip parameter
            AddressState = aState; // sets AddressState variable to the aState parameter
            AddressCity = aCity; // sets AddressCity variable to the aCity parameter
            AddressStreet = address; // sets AddressStreet variable to the address parameter
            PropertyType = type; // sets PropertyType variable to the type parameter
            PropertySize = size; // sets PropertySize variable to the size parameter
            NumberOfBedrooms = bedrooms; // sets NumberOfBedrooms variable to the bedrooms parameter
            NumberOfBathrooms = bathrooms; // sets NumberOfBathrooms variable to the bathrooms parameter
            GarageType = garage; // sets GarageType variable to the garage parameter
            HeatingSystem = heating; // sets HeatingSystem variable to the heating parameter
            CoolingSystem = cooling; // sets CoolingSystem variable to the cooling parameter
            YearBuilt = yBuilt; // sets YearBuilt variable to the yBuilt parameter
            UtilitiesWater = water; // sets UtilitiesWater variable to the water parameter
            UtilitiesSewer = sewer; // sets UtilitiesSewer variable to the sewer parameter
            PropertyDescription = description; // sets PropertyDescription variable to the description parameter
            AskingPrice = price; // sets AskingPrice variable to the price parameter
        }


        public Property(Dictionary<string, string> propertyInfo, decimal propertySize) // public Property initalizer that a dictionary of strings and a decimal as parameters
        {
            AccountID = int.Parse(propertyInfo["AgentID"]); // sets AccountID variable to the converted int value of the value from the AgentID key in the propertyInfo parameter
            AddressZipCode = int.Parse(propertyInfo["ZipAddress"]); // sets AddressZipCode variable to the converted int value of the value from the ZipAddress key in the propertyInfo parameter
            AddressState = propertyInfo["StateAddress"]; // sets AddressState variable to the value from the StateAddress key in the propertyInfo parameter
            AddressCity = propertyInfo["CityAddress"]; // sets AddressCity variable to the value from the CityAddress key in the propertyInfo parameter
            AddressStreet = propertyInfo["StreetAddress"]; // sets AddressStreet variable to the value from the StreetAddress key in the propertyInfo parameter
            PropertyType = propertyInfo["Type"]; // sets PropertyType variable to the value from the Type key in the propertyInfo parameter
            PropertySize = propertySize; // sets PropertySize variable to the propertySize parameter
            NumberOfBedrooms = int.Parse(propertyInfo["Bedrooms"]); // sets NumberOfBedrooms variable to the converted int value of the value from the Bedrooms key in the propertyInfo parameter
            NumberOfBathrooms = decimal.Parse(propertyInfo["Bathrooms"]); // sets NumberOfBathrooms variable to the converted decimal value of the value from the Bathrooms key in the propertyInfo parameter
            GarageType = propertyInfo["Garage"]; // sets GarageType variable to the value from the Garage key in the propertyInfo parameter
            HeatingSystem = propertyInfo["Heating"]; // sets HeatingSystem variable to the value from the Heating key in the propertyInfo parameter
            CoolingSystem = propertyInfo["Cooling"]; // sets CoolingSystem variable to the value from the Cooling key in the propertyInfo parameter
            YearBuilt = int.Parse(propertyInfo["YearBuilt"]); // sets YearBuilt variable to the converted int value of the value from the YearBuilt key in the propertyInfo parameter
            UtilitiesWater = propertyInfo["Water"]; // sets UtilitiesWater variable to the value from the Water key in the propertyInfo parameter
            UtilitiesSewer = propertyInfo["Sewer"]; // sets UtilitiesSewer variable to the value from the Sewer key in the propertyInfo parameter
            PropertyDescription = propertyInfo["Description"]; // sets PropertyDescription variable to the value from the Description key in the propertyInfo parameter
            AskingPrice = decimal.Parse(propertyInfo["AskingPrice"]); // sets AskingPrice variable to the converted decimal value of the value from the AskingPrice key in the propertyInfo parameter
        }
    }
}
