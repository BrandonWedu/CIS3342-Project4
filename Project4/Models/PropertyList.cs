using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public class PropertyList
    {
        private List<Property> properties = new List<Property>(); // private List of Property objects called properties set to a new list of Property
        private List<Property> activeProperties = new List<Property>(); // private List of Property objects called activeProperties set to a new list of Property
        private DBConnect databaseHandler = new DBConnect(); // private DBConnect object called databaseHandler set to a new DBConnect object

        public PropertyList() // public PropertyList object initializer
        {
            properties = new List<Property>(); // sets properties to a new List of Property objects
            SqlCommand selectProcedure = new SqlCommand(); // SqlCommand object called selectProcedure set to a new SqlCommand object
            selectProcedure.CommandType = CommandType.StoredProcedure; // sets selectProceudre's CommandType to the StoredProcedure command type
            selectProcedure.CommandText = "SelectAllProperties"; // sets selectProceudre's CommandText to SelectAllProperties
            DataTable propertyData = databaseHandler.GetDataSet(selectProcedure).Tables[0]; // DataTable object called propertyData set to the first table of the returned value of GetDataSet function from the databaseHandler object with the selectProceudre as a parameter

            foreach (DataRow row in propertyData.Rows)  // foreach DataRow called row in propertyData's rows do
            {
                properties.Add(new Property((int)row["PropertyID"], (int)row["AccountID"], (int)row["AddressZipCode"], row["AddressState"].ToString(), row["AddressCity"].ToString(), row["AddressStreet"].ToString(), row["PropertyType"].ToString(), (decimal)row["PropertySize"], (int)row["NumberOfBedrooms"], (decimal)row["NumberOfBathrooms"], row["GarageType"].ToString(), row["HeatingSystem"].ToString(), row["CoolingSystem"].ToString(), (int)row["YearBuilt"], row["UtilitiesWater"].ToString(), row["UtilitiesSewer"].ToString(), row["PropertyDescription"].ToString(), (decimal)row["AskingPrice"], (DateTime)row["CreationDate"])); // adds a new Property object to properties list object with the row's PropertyID column value converted to an int, the row's AccountID column value converted to an int, the row's AddressZipCode column value converted to an int, the row's AddressState column value converted to a string, the row's AddressCity column value converted to a string, the row's AddressStreet column value converted to a string, the row's PropertyType column converted to a string, the row's PropertySize column converted to a decimal, the row's NumberOfBedrooms column converted to a int, the row's NumberOfBathrooms column converted to a int, the row's GarageType column converted to a int, the row's HeatingSystem column converted to a string, the row's CoolingSystem column converted to a string, the row's YearBuilt column converted to a int, the row's UtilitiesWater column converted to a string, the row's UtilitiesSewer column converted to a string, the row's PropertyDescription column converted to a string, the row's AskingPrice column converted to a decimal, and the row's CreationDate column converted to a DateTime
            }
        }

        public PropertyList(int agentID) // public PropertyList object initializer that accepts a int parameter
        {
            properties = new List<Property>(); // sets properties to a new List of Property objects
            SqlCommand selectProcedure = new SqlCommand(); // SqlCommand object called selectProcedure set to a new SqlCommand object
            selectProcedure.CommandType = CommandType.StoredProcedure; // sets selectProceudre's CommandType to the StoredProcedure command type
            selectProcedure.CommandText = "SelectAllPropertiesByAgentID"; // sets selectProceudre's CommandText to SelectAllPropertiesByAgentID
            selectProcedure.Parameters.AddWithValue("@AccountID", agentID); // Adds a parameter with value to the selectProceudre object with @PropertyID as the name and the agentID parameter as the value
            DataTable propertyData = databaseHandler.GetDataSet(selectProcedure).Tables[0]; // DataTable object called propertyData set to the first table of the returned value of GetDataSet function from the databaseHandler object with the selectProceudre as a parameter

            foreach (DataRow row in propertyData.Rows) // foreach DataRow called row in propertyData's rows do
            {
                properties.Add(new Property((int)row["PropertyID"], (int)row["AccountID"], (int)row["AddressZipCode"], row["AddressState"].ToString(), row["AddressCity"].ToString(), row["AddressStreet"].ToString(), row["PropertyType"].ToString(), (decimal)row["PropertySize"], (int)row["NumberOfBedrooms"], (decimal)row["NumberOfBathrooms"], row["GarageType"].ToString(), row["HeatingSystem"].ToString(), row["CoolingSystem"].ToString(), (int)row["YearBuilt"], row["UtilitiesWater"].ToString(), row["UtilitiesSewer"].ToString(), row["PropertyDescription"].ToString(), (decimal)row["AskingPrice"], (DateTime)row["CreationDate"])); // adds a new Property object to properties list object with the row's PropertyID column value converted to an int, the row's AccountID column value converted to an int, the row's AddressZipCode column value converted to an int, the row's AddressState column value converted to a string, the row's AddressCity column value converted to a string, the row's AddressStreet column value converted to a string, the row's PropertyType column converted to a string, the row's PropertySize column converted to a decimal, the row's NumberOfBedrooms column converted to a int, the row's NumberOfBathrooms column converted to a int, the row's GarageType column converted to a int, the row's HeatingSystem column converted to a string, the row's CoolingSystem column converted to a string, the row's YearBuilt column converted to a int, the row's UtilitiesWater column converted to a string, the row's UtilitiesSewer column converted to a string, the row's PropertyDescription column converted to a string, the row's AskingPrice column converted to a decimal, and the row's CreationDate column converted to a DateTime
            }
        }

        public Property GetPropertyByPropertyID(int id) // public GetPropertyByPropertyId function that accepts a int parameter
        {
            int count = 0; // int variable called count set to zero
            int validCount = 0; // int variable called valid count set to zero
            foreach (Property property in properties) // foreach Property object called property in the properties List object do
            {
                if (property.PropertyID == id) // if the current property objets PropertyID is equal to the passed parameter then
                {
                    validCount = count; // sets validCount to the value of the count variable
                }
                count++; // adds one to the current value of count
            }

            return properties[validCount]; // returns the Property object at the validCount index of the properties List object
        }

        /*
        public List<Property> GetAllActiveProperties(List<PropertyStatus> activePropertyStatus) // public GetAllActiveProperties function that returns a List of Property objects and accepts a List of PropertyStatus objects
        {
            activeProperties = new List<Property>(); // sets activeProperties to a new List of Property object
            SqlCommand selectProcedure = new SqlCommand(); // SqlCommand object called selectProcedure set to a new SqlCommand object
            selectProcedure.CommandType = CommandType.StoredProcedure; // sets selectProceudre's CommandType to the StoredProcedure command type
            selectProcedure.CommandText = "SelectAllProperties"; // sets selectProceudre's CommandText to SelectAllProperties
            DataTable propertyData = databaseHandler.GetDataSet(selectProcedure).Tables[0]; // Adds a parameter with value to the selectProceudre object with @PropertyID as the name and the agentID parameter as the value

            foreach (DataRow row in propertyData.Rows) // foreach DataRow called row in propertyData's rows do
            {
                foreach (PropertyStatus status in activePropertyStatus) // foreach PropertyStatus object called status in activePropertyStatus List object do
                {
                    if (status.CurrentStatus == "Active" && (int)row["PropertyID"] == status.PropertyID) // if the current status objects CurrentStatus is equal to Active and if the current rows PropertyID column converted to an int is equal to the passed PropertyID parameter then
                    {
                        activeProperties.Add(new Property((int)row["PropertyID"], (int)row["AccountID"], (int)row["AddressZipCode"], row["AddressState"].ToString(), row["AddressCity"].ToString(), row["AddressStreet"].ToString(), row["PropertyType"].ToString(), (decimal)row["PropertySize"], (int)row["NumberOfBedrooms"], (decimal)row["NumberOfBathrooms"], row["GarageType"].ToString(), row["HeatingSystem"].ToString(), row["CoolingSystem"].ToString(), (int)row["YearBuilt"], row["UtilitiesWater"].ToString(), row["UtilitiesSewer"].ToString(), row["PropertyDescription"].ToString(), (decimal)row["AskingPrice"], (DateTime)row["CreationDate"])); // adds a new Property object to properties list object with the row's PropertyID column value converted to an int, the row's AccountID column value converted to an int, the row's AddressZipCode column value converted to an int, the row's AddressState column value converted to a string, the row's AddressCity column value converted to a string, the row's AddressStreet column value converted to a string, the row's PropertyType column converted to a string, the row's PropertySize column converted to a decimal, the row's NumberOfBedrooms column converted to a int, the row's NumberOfBathrooms column converted to a int, the row's GarageType column converted to a int, the row's HeatingSystem column converted to a string, the row's CoolingSystem column converted to a string, the row's YearBuilt column converted to a int, the row's UtilitiesWater column converted to a string, the row's UtilitiesSewer column converted to a string, the row's PropertyDescription column converted to a string, the row's AskingPrice column converted to a decimal, and the row's CreationDate column converted to a DateTime
                    }
                }
            }

            return activeProperties; // returns the activeProperties List object
        }
        */

        public List<Property> GetPropertyListByPropertyID(int id) // public GetPropertyListByPropertyID function that accepts a int parameter and returns a List of Property object
        {
            properties = new List<Property>(); // sets properties to a new List of Property object
            SqlCommand selectProcedure = new SqlCommand(); // SqlCommand object called selectProcedure set to a new SqlCommand object
            selectProcedure.CommandType = CommandType.StoredProcedure; // sets selectProceudre's CommandType to the StoredProcedure command type
            selectProcedure.CommandText = "SelectAllProperties"; // sets selectProceudre's CommandText to SelectAllProperties
            DataTable propertyData = databaseHandler.GetDataSet(selectProcedure).Tables[0]; // DataTable object called propertyData set to the first table of the returned value of GetDataSet function from the databaseHandler object with the selectProceudre as a parameter

            foreach (DataRow row in propertyData.Rows) // foreach DataRow called row in propertyData's rows do
            {
                if ((int)row["PropertyID"] == id) // if the current rows PropertyID column converted to an int is equal to the passed id parameter then
                {
                    properties.Add(new Property((int)row["PropertyID"], (int)row["AccountID"], (int)row["AddressZipCode"], row["AddressState"].ToString(), row["AddressCity"].ToString(), row["AddressStreet"].ToString(), row["PropertyType"].ToString(), (decimal)row["PropertySize"], (int)row["NumberOfBedrooms"], (decimal)row["NumberOfBathrooms"], row["GarageType"].ToString(), row["HeatingSystem"].ToString(), row["CoolingSystem"].ToString(), (int)row["YearBuilt"], row["UtilitiesWater"].ToString(), row["UtilitiesSewer"].ToString(), row["PropertyDescription"].ToString(), (decimal)row["AskingPrice"], (DateTime)row["CreationDate"])); // adds a new Property object to properties list object with the row's PropertyID column value converted to an int, the row's AccountID column value converted to an int, the row's AddressZipCode column value converted to an int, the row's AddressState column value converted to a string, the row's AddressCity column value converted to a string, the row's AddressStreet column value converted to a string, the row's PropertyType column converted to a string, the row's PropertySize column converted to a decimal, the row's NumberOfBedrooms column converted to a int, the row's NumberOfBathrooms column converted to a int, the row's GarageType column converted to a int, the row's HeatingSystem column converted to a string, the row's CoolingSystem column converted to a string, the row's YearBuilt column converted to a int, the row's UtilitiesWater column converted to a string, the row's UtilitiesSewer column converted to a string, the row's PropertyDescription column converted to a string, the row's AskingPrice column converted to a decimal, and the row's CreationDate column converted to a DateTime
                }

            }
            return properties; // returns properties list object
        }

        public Property GetPropertyByStreetAddressAndZipCode(string address, int zip) // public GetPropertyByStreetAddressAndZipCode function that returns a property object and accepts a string and int parameter
        {
            properties = new List<Property>(); // sets properties to a new List of Property object
            SqlCommand selectProcedure = new SqlCommand(); // SqlCommand object called selectProcedure set to a new SqlCommand object
            selectProcedure.CommandType = CommandType.StoredProcedure; // sets selectProceudre's CommandType to the StoredProcedure command type
            selectProcedure.CommandText = "SelectPropertyByZipAndStreetAddress"; // sets selectProceudre's CommandText to SelectPropertyByZipAndStreetAddress
            selectProcedure.Parameters.AddWithValue("@AddressStreet", address); // Adds a parameter with value to the selectProceudre object with @AddressStreet as the name and the address parameter as the value
            selectProcedure.Parameters.AddWithValue("@AddressZipCode", zip); // Adds a parameter with value to the selectProceudre object with @AddressZipCode as the name and the zip parameter as the value
            DataTable propertyTable = databaseHandler.GetDataSet(selectProcedure).Tables[0]; // DataTable object called propertyData set to the first table of the returned value of GetDataSet function from the databaseHandler object with the selectProceudre as a parameter

            foreach (DataRow row in propertyTable.Rows) // foreach DataRow called row in propertyData's rows do
            {
                properties.Add(new Property((int)row["PropertyID"], (int)row["AccountID"], (int)row["AddressZipCode"], row["AddressState"].ToString(), row["AddressCity"].ToString(), row["AddressStreet"].ToString(), row["PropertyType"].ToString(), (decimal)row["PropertySize"], (int)row["NumberOfBedrooms"], (decimal)row["NumberOfBathrooms"], row["GarageType"].ToString(), row["HeatingSystem"].ToString(), row["CoolingSystem"].ToString(), (int)row["YearBuilt"], row["UtilitiesWater"].ToString(), row["UtilitiesSewer"].ToString(), row["PropertyDescription"].ToString(), (decimal)row["AskingPrice"], (DateTime)row["CreationDate"])); // adds a new Property object to properties list object with the row's PropertyID column value converted to an int, the row's AccountID column value converted to an int, the row's AddressZipCode column value converted to an int, the row's AddressState column value converted to a string, the row's AddressCity column value converted to a string, the row's AddressStreet column value converted to a string, the row's PropertyType column converted to a string, the row's PropertySize column converted to a decimal, the row's NumberOfBedrooms column converted to a int, the row's NumberOfBathrooms column converted to a int, the row's GarageType column converted to a int, the row's HeatingSystem column converted to a string, the row's CoolingSystem column converted to a string, the row's YearBuilt column converted to a int, the row's UtilitiesWater column converted to a string, the row's UtilitiesSewer column converted to a string, the row's PropertyDescription column converted to a string, the row's AskingPrice column converted to a decimal, and the row's CreationDate column converted to a DateTime
            }

            return properties[0]; // returns the first object in the properties List object
        }

        public List<Property> GetAllProperties() // public GetAllProperties function that returns a List of Property object
        {
            return properties; // returns the properties List object
        }

        public void AddNewProperty(Property propertyToBeAdded) // public AddNewProperty function that accepts a Property object
        {
            DateTime now = DateTime.Now; // DateTime object called now set to the current date and time
            SqlCommand insertProcedure = new SqlCommand(); // SqlCommand object called insertProcedure set to a new SqlCommand object
            insertProcedure.CommandType = CommandType.StoredProcedure; // sets insertProcedure's CommandType to the StoredProcedure command type
            insertProcedure.CommandText = "InsertNewProperty"; // sets insertProcedure's CommandText to InsertNewProperty
            insertProcedure.Parameters.AddWithValue("@AccountID", propertyToBeAdded.AccountID); // Adds a parameter with value to the insertProcedure object with @AccountID as the name and the propertyToBeAdded objects AccountID as the value
            insertProcedure.Parameters.AddWithValue("@AddressZipCode", propertyToBeAdded.AddressZipCode); // Adds a parameter with value to the insertProcedure object with @AddressZipCode as the name and the propertyToBeAdded objects AddressZipCode as the value
            insertProcedure.Parameters.AddWithValue("@AddressState", propertyToBeAdded.AddressState); // Adds a parameter with value to the insertProcedure object with @AddressState as the name and the propertyToBeAdded objects AddressState as the value
            insertProcedure.Parameters.AddWithValue("@AddressCity", propertyToBeAdded.AddressCity); // Adds a parameter with value to the insertProcedure object with @AddressCity as the name and the propertyToBeAdded objects AddressCity as the value
            insertProcedure.Parameters.AddWithValue("@AddressStreet", propertyToBeAdded.AddressStreet); // Adds a parameter with value to the insertProcedure object with @AddressStreet as the name and the propertyToBeAdded objects AddressStreet as the value
            insertProcedure.Parameters.AddWithValue("@PropertyType", propertyToBeAdded.PropertyType); // Adds a parameter with value to the insertProcedure object with @PropertyType as the name and the propertyToBeAdded objects PropertyType as the value
            insertProcedure.Parameters.AddWithValue("@PropertySize", propertyToBeAdded.PropertySize); // Adds a parameter with value to the insertProcedure object with @PropertySize as the name and the propertyToBeAdded objects PropertySize as the value
            insertProcedure.Parameters.AddWithValue("@NumberOfBedrooms", propertyToBeAdded.NumberOfBedrooms); // Adds a parameter with value to the insertProcedure object with @NumberOfBedrooms as the name and the propertyToBeAdded objects NumberOfBedrooms as the value
            insertProcedure.Parameters.AddWithValue("@NumberOfBathrooms", propertyToBeAdded.NumberOfBathrooms); // Adds a parameter with value to the insertProcedure object with @NumberOfBathrooms as the name and the propertyToBeAdded objects NumberOfBathrooms as the value
            insertProcedure.Parameters.AddWithValue("@GarageType", propertyToBeAdded.GarageType); // Adds a parameter with value to the insertProcedure object with @GarageType as the name and the propertyToBeAdded objects GarageType as the value
            insertProcedure.Parameters.AddWithValue("@HeatingSystem", propertyToBeAdded.HeatingSystem); // Adds a parameter with value to the insertProcedure object with @HeatingSystem as the name and the propertyToBeAdded objects HeatingSystem as the value
            insertProcedure.Parameters.AddWithValue("@CoolingSystem", propertyToBeAdded.CoolingSystem); // Adds a parameter with value to the insertProcedure object with @CoolingSystem as the name and the propertyToBeAdded objects CoolingSystem as the value
            insertProcedure.Parameters.AddWithValue("@YearBuilt", propertyToBeAdded.YearBuilt); // Adds a parameter with value to the insertProcedure object with @YearBuilt as the name and the propertyToBeAdded objects YearBuilt as the value
            insertProcedure.Parameters.AddWithValue("@UtilitiesWater", propertyToBeAdded.UtilitiesWater); // Adds a parameter with value to the insertProcedure object with @UtilitiesWater as the name and the propertyToBeAdded objects UtilitiesWater as the value
            insertProcedure.Parameters.AddWithValue("@UtilitesSewer", propertyToBeAdded.UtilitiesSewer); // Adds a parameter with value to the insertProcedure object with @UtilitiesSewer as the name and the propertyToBeAdded objects UtilitiesSewer as the value
            insertProcedure.Parameters.AddWithValue("@PropertyDescription", propertyToBeAdded.PropertyDescription); // Adds a parameter with value to the insertProcedure object with @PropertyDescription as the name and the propertyToBeAdded objects PropertyDescription as the value
            insertProcedure.Parameters.AddWithValue("@AskingPrice", propertyToBeAdded.AskingPrice); // Adds a parameter with value to the insertProcedure object with @AskingPrice as the name and the propertyToBeAdded objects AskingPrice as the value
            insertProcedure.Parameters.AddWithValue("@CreationDate", now.Date); // Adds a parameter with value to the insertProcedure object with @AskingPrice as the name and the now DateTime object's date as the value
            databaseHandler.DoUpdate(insertProcedure); // calls the DoUpdate function of the databaseHandler object with inserProcedure as a parameter
        }

        public void RemoveProperty(int propertyID) // public RemoveProperty function that accepts a int parameter
        {
            //SqlCommand deleteProcedure = new SqlCommand(); // SqlCommand object called deleteProcedure set to a new SqlCommand object
            //deleteProcedure.CommandType = CommandType.StoredProcedure; // sets deleteProcedure's CommandType to the StoredProcedure command type
            //deleteProcedure.CommandText = "DeleteImagesByPropertyID"; // sets deleteProcedure's CommandText to DeleteImagesByPropertyID
            //deleteProcedure.Parameters.AddWithValue("@PropertyID", propertyID); // Adds a parameter with value to the deleteProcedure object with @PropertyID as the name and the propertyID parameter as the value
            //databaseHandler.DoUpdate(deleteProcedure); // calls the DoUpdate function of the databaseHandler object with deleteProcedure as a parameter

            SqlCommand deleteProcedure = new SqlCommand(); // sets deleteProcedure to a new SqlCommand object
            deleteProcedure.CommandType = CommandType.StoredProcedure; // sets deleteProcedure's CommandType to the StoredProcedure command type
            deleteProcedure.CommandText = "DeleteRoomsByPropertyID"; // sets deleteProcedure's CommandText to DeleteRoomsByPropertyID
            deleteProcedure.Parameters.AddWithValue("@PropertyID", propertyID); // Adds a parameter with value to the deleteProcedure object with @PropertyID as the name and the propertyID parameter as the value
            databaseHandler.DoUpdate(deleteProcedure); // calls the DoUpdate function of the databaseHandler object with deleteProcedure as a parameter

            deleteProcedure = new SqlCommand(); // sets deleteProcedure to a new SqlCommand object
            deleteProcedure.CommandType = CommandType.StoredProcedure; // sets deleteProcedure's CommandType to the StoredProcedure command type
            deleteProcedure.CommandText = "DeletePropertiesStatusByPropertyID"; // sets deleteProcedure's CommandText to DeletePropertiesStatusByPropertyID
            deleteProcedure.Parameters.AddWithValue("@PropertyID", propertyID); // Adds a parameter with value to the deleteProcedure object with @PropertyID as the name and the propertyID parameter as the value
            databaseHandler.DoUpdate(deleteProcedure); // calls the DoUpdate function of the databaseHandler object with deleteProcedure as a parameter


            deleteProcedure = new SqlCommand(); // sets deleteProcedure to a new SqlCommand object
            deleteProcedure.CommandType = CommandType.StoredProcedure; // sets deleteProcedure's CommandType to the StoredProcedure command type
            deleteProcedure.CommandText = "DeletePropertyAmenitiesByPropertyId"; // sets deleteProcedure's CommandText to DeletePropertyAmenitiesByPropertyId
            deleteProcedure.Parameters.AddWithValue("@PropertyID", propertyID); // Adds a parameter with value to the deleteProcedure object with @PropertyID as the name and the propertyID parameter as the value
            databaseHandler.DoUpdate(deleteProcedure); // calls the DoUpdate function of the databaseHandler object with deleteProcedure as a parameter

            deleteProcedure = new SqlCommand(); // sets deleteProcedure to a new SqlCommand object
            deleteProcedure.CommandType = CommandType.StoredProcedure; // sets deleteProcedure's CommandType to the StoredProcedure command type
            deleteProcedure.CommandText = "DeletePropertiesPriceHistoryByOfferID"; // sets deleteProcedure's CommandText to DeletePropertiesPriceHistoryByOfferID
            deleteProcedure.Parameters.AddWithValue("@PropertyID", propertyID); // Adds a parameter with value to the deleteProcedure object with @PropertyID as the name and the propertyID parameter as the value
            databaseHandler.DoUpdate(deleteProcedure); // calls the DoUpdate function of the databaseHandler object with deleteProcedure as a parameter

            // This one needs to be last 
            deleteProcedure = new SqlCommand(); // sets deleteProcedure to a new SqlCommand object
            deleteProcedure.CommandType = CommandType.StoredProcedure; // sets deleteProcedure's CommandType to the StoredProcedure command type
            deleteProcedure.CommandText = "DeletePropertiesByPropertyID"; // sets deleteProcedure's CommandText to DeletePropertiesByPropertyID
            deleteProcedure.Parameters.AddWithValue("@PropertyID", propertyID); // Adds a parameter with value to the deleteProcedure object with @PropertyID as the name and the propertyID parameter as the value
            databaseHandler.DoUpdate(deleteProcedure); // calls the DoUpdate function of the databaseHandler object with deleteProcedure as a parameter

        }

        public void UpdateProperty(int propertyID, Dictionary<string, string> updatedInfo, decimal totalSqFt) // public UpdateProperty function that accepts a int, a dictionary of strings, and a decimal parameter
        {
            SqlCommand updateProcedure = new SqlCommand(); // SqlCommand object called updateProcedure set to a new SqlCommand object
            updateProcedure.CommandType = CommandType.StoredProcedure; // sets updateProcedure's CommandType to the StoredProcedure command type
            updateProcedure.CommandText = "UpdatePropertyInformationByPropertyID"; // sets updateProcedure's CommandText to UpdatePropertyInformationByPropertyID
            updateProcedure.Parameters.AddWithValue("@PropertyID", propertyID); // Adds a parameter with value to the updateProcedure object with @PropertyID as the name and the propertyID parameter as the value
            updateProcedure.Parameters.AddWithValue("@AddressZipCode", int.Parse(updatedInfo["ZipAddress"])); // Adds a parameter with value to the updateProcedure object with @AddressZipCode as the name and the value converted to an int from the ZipAddress key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@AddressState", updatedInfo["StateAddress"]); // Adds a parameter with value to the updateProcedure object with @AddressState as the name and the value from the StateAddress key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@AddressCity", updatedInfo["CityAddress"]); // Adds a parameter with value to the updateProcedure object with @AddressCity as the name and the value from the AddressCity key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@AddressStreet", updatedInfo["StreetAddress"]); // Adds a parameter with value to the updateProcedure object with @AddressStreet as the name and the value from the StreetAddress key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@PropertyType", updatedInfo["Type"]); // Adds a parameter with value to the updateProcedure object with @PropertyType as the name and the value from the Type key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@PropertySize", totalSqFt); // Adds a parameter with value to the updateProcedure object with @PropertyID as the name and the totalSqFt parameter as the value
            updateProcedure.Parameters.AddWithValue("@NumberOfBedrooms", int.Parse(updatedInfo["Bedrooms"])); // Adds a parameter with value to the updateProcedure object with @NumberOfBedrooms as the name and the value converted to an int from the Bedrooms key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@NumberOfBathrooms", decimal.Parse(updatedInfo["Bathrooms"])); // Adds a parameter with value to the updateProcedure object with @NumberOfBathrooms as the name and the value converted to a decimal from the Bathrooms key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@GarageType", int.Parse(updatedInfo["Garage"])); // Adds a parameter with value to the updateProcedure object with @GarageType as the name and the value converted to an int from the Garage key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@HeatingSystem", updatedInfo["Heating"]); // Adds a parameter with value to the updateProcedure object with @HeatingSystem as the name and the value from the Heating key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@CoolingSystem", updatedInfo["Cooling"]); // Adds a parameter with value to the updateProcedure object with @CoolingSystem as the name and the value from the Cooling key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@YearBuilt", int.Parse(updatedInfo["YearBuilt"])); // Adds a parameter with value to the updateProcedure object with @YearBuilt as the name and the value converted to an int from the YearBuilt key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@UtilitiesWater", updatedInfo["Water"]); // Adds a parameter with value to the updateProcedure object with @UtilitiesWater as the name and the value from the Water key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@UtilitesSewer", updatedInfo["Sewer"]); // Adds a parameter with value to the updateProcedure object with @UtilitesSewer as the name and the value from the Sewer key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@PropertyDescription", updatedInfo["Description"]); // Adds a parameter with value to the updateProcedure object with @PropertyDescription as the name and the value from the Description key in the updatedInfo dictionary as the value
            updateProcedure.Parameters.AddWithValue("@AskingPrice", decimal.Parse(updatedInfo["AskingPrice"])); // Adds a parameter with value to the updateProcedure object with @AskingPrice as the name and the value converted to a decimal from the AskingPrice key in the updatedInfo dictionary as the value
            databaseHandler.DoUpdate(updateProcedure); // calls the DoUpdate function from the databaseHandler object with the updateProcedure object as a parameter
        }

        /*
        public List<Property> GetFilteredPropertyList(List<PropertyAmenity> allPropertyAmenities, Dictionary<string, string> filterInputs, List<int> filterAmenityIDs, bool[] selectedFilters) // public GetFilteredPropertyList function that returns a List of Property object and accepts a List of PropertyAmenity, a dictionary of strings, A List of ints, and a bool array
        {
            List<Property> filteredProperties = new List<Property>(); // List of Property object called filteredProperties set to a new List of Property object

            foreach (Property listing in activeProperties) // foreach Property object called listing in activeProperties List object do
            {
                bool matchesAllSelectedFilters = true; // bool variable called matchesAllSelectedFilters set to true
                List<int> propertyAmenities = new List<int>(); // List of ints called propertyAmenities set to a new List of ints

                if (selectedFilters[0] == true && filterInputs["FilterCity"] != "") // if the first bool in selectedFilters is true and the value of the FilterCity key in filterInputs dictionary is not equal to an empty string then
                {
                    if (listing.AddressCity.ToLower() != filterInputs["FilterCity"].ToLower()) // if the current listing objects AddressCity to lowercase is not equal to the FilterCity key in filterInputs dictionary to lowercase then
                    {
                        matchesAllSelectedFilters = false; // set matchesAllSelectedFilters to false
                    }
                }

                if (selectedFilters[1] == true && filterInputs["FilterState"] != "") // if the second bool in selectedFilters is true and the value of the FilterState key in filterInputs dictionary is not equal to an empty string then
                {
                    if (listing.AddressState.ToLower() != filterInputs["FilterState"].ToLower()) // if the current listing objects AddressState to lowercase is not equal to the FilterState key in filterInputs dictionary to lowercase then
                    {
                        matchesAllSelectedFilters = false; // set matchesAllSelectedFilters to false
                    }
                }

                if (selectedFilters[2] == true && filterInputs["FilterPropertyType"] != "") // if the third bool in selectedFilters is true and the value of the FilterPropertyType key in filterInputs dictionary is not equal to an empty string then
                {
                    if (listing.PropertyType.ToLower() != filterInputs["FilterPropertyType"].ToLower()) // if the current listing objects PropertyType to lowercase is not equal to the FilterPropertyType key in filterInputs dictionary to lowercase then
                    {
                        matchesAllSelectedFilters = false; // set matchesAllSelectedFilters to false
                    }
                }

                if (selectedFilters[3] == true) // if the fourth bool in selectedFilters is true then
                {
                    decimal minPrice = decimal.Parse(filterInputs["FilterMinPrice"]); // decimal variable called minPrice set to the converted decimal of the value of the FilterMinPrice key in filterInputs dictionary
                    decimal maxPrice = decimal.Parse(filterInputs["FilterMaxPrice"]); // decimal variable called maxPrice set to the converted decimal of the value of the FilterMaxPrice key in filterInputs dictionary
                    if (listing.AskingPrice < minPrice || listing.AskingPrice > maxPrice) // if the current listing objects AskingPrice is less than the minPrice variable or if the current listing objects AskingPrice is greater than the maxPrice variable then
                    {
                        matchesAllSelectedFilters = false; // set matchesAllSelectedFilters to false
                    }
                }

                if (selectedFilters[4] == true && filterInputs["FilterSize"] != "") // if the fifth bool in selectedFilters is true and the value of the FilterSize key in filterInputs dictionary is not equal to an empty string then
                {
                    decimal propertyMinSize = decimal.Parse(filterInputs["FilterSize"]); // decimal variable called propertyMinSize set to the converted decimal of the value of the FilterSize key in filterInputs dictionary
                    if (listing.PropertySize < propertyMinSize) // if the current listing objects PropertySize is less than propertyMinSize then
                    {
                        matchesAllSelectedFilters = false; // set matchesAllSelectedFilters to false
                    }
                }

                if (selectedFilters[5] == true && filterInputs["FilterMinBedroom"] != "") // if the sixth bool in selectedFilters is true and the value of the FilterMinBedroom key in filterInputs dictionary is not equal to an empty string then
                {
                    int minBedrooms = int.Parse(filterInputs["FilterMinBedroom"]); // int variable called minBedrooms set to the converted int of the value of the FilterMinBedroom disctionary
                    if (listing.NumberOfBedrooms < minBedrooms) // if the current listing objects NumberOfBedrooms is less than minBedrooms variable then
                    {
                        matchesAllSelectedFilters = false; // set matchesAllSelectedFilters to false
                    }
                }

                if (selectedFilters[6] == true && filterInputs["FilterMinBathroom"] != "") // if the seventh bool in selectedFilters is true and the value of the FilterMinBathroom key in filterInputs dictionary is not equal to an empty string then
                {
                    decimal minBathroom = decimal.Parse(filterInputs["FilterMinBathroom"]); // decimal variable called minBedrooms set to the converted decimal of the value of the FilterMinBathroom disctionary
                    if (listing.NumberOfBathrooms < minBathroom) // if the current listing objects NumberOfBathrooms is less than minBathroom variable then
                    {
                        matchesAllSelectedFilters = false; // set matchesAllSelectedFilters to false
                    }
                }

                if (selectedFilters[7] == true) // if the eighth bool in selectedFilters is true then
                {
                    foreach (PropertyAmenity amenity in allPropertyAmenities) // foreach PropertyAmenity object called amenity in allPropertyAmenities list object do
                    {
                        if (amenity.PropertyID == listing.PropertyID) // if the current amenity objects PropertyID is equal to the current listings PropertyID then
                        {
                            foreach (int amenitiyID in filterAmenityIDs) // foreach int called amenitiyID in the filterAmenityIDs list object do
                            {
                                if (amenitiyID == amenity.AmenityID) // if the current amenitiyID is equal to the current amenitiy objects AmenityId then
                                {
                                    propertyAmenities.Add(amenitiyID); // adds the current amenitiyID to the propertyAmenitites list object
                                }
                            }
                        }
                    }

                    if (propertyAmenities.Count != filterAmenityIDs.Count) // if the propertyAmenities List objects count is not equal to the filterAmenityIDs list objects count then
                    {
                        matchesAllSelectedFilters = false; // set matchesAllSelectedFilters to false
                    }
                }

                if (matchesAllSelectedFilters == true) // if matchesAllSelectedFilters is true then
                {
                    filteredProperties.Add(listing); // adds the current listing object to the filteredProperties List object
                }

            }

            return filteredProperties; // returns the filteredProperties list object
        }
        */
    }
}
