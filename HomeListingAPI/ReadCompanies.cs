using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
    public class ReadCompanies
    {

        internal static Companies ReadAllCompanies()
        {
            DBConnect databaseHandler = new DBConnect();
            Companies allCompanies = new Companies();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectCompanies";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allCompanies.Add(new Company((int)row["CompanyID"], row["CompanyName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["CompanyAddress"]), row["CompanyPhoneNumber"].ToString(), row["CompanyEmail"].ToString()));
            }

            return allCompanies;
        }


        internal static Companies GetCompanyByCompanyID(int id)
        {
            DBConnect databaseHandler = new DBConnect();
            Companies allCompanies = new Companies();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectCompanies";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allCompanies.Add(new Company((int)row["CompanyID"], row["CompanyName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["CompanyAddress"]), row["CompanyPhoneNumber"].ToString(), row["CompanyEmail"].ToString()));
            }
            Companies selectedCompany = new Companies();
            foreach (Company currentCompany in allCompanies.List)
            {
                if (currentCompany.CompanyID == id)
                {
                    selectedCompany.Add(currentCompany);
                }
            }
            return selectedCompany;
        }

        internal static Companies GetComapnyByNameAndAddress(string name, Address address)
        {
            DBConnect databaseHandler = new DBConnect();
            Companies allCompanies = new Companies();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_SelectCompanies";
            DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

            foreach (DataRow row in agentContactData.Rows)
            {
                allCompanies.Add(new Company((int)row["CompanyID"], row["CompanyName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["CompanyAddress"]), row["CompanyPhoneNumber"].ToString(), row["CompanyEmail"].ToString()));
            }
            Companies selectedCompany = new Companies();
            foreach (Company currentCompany in allCompanies.List)
            {
                if (currentCompany.CompanyName == name)
                {
                    if (currentCompany.CompanyAddress.Street == address.Street)
                    {
                        selectedCompany.Add(currentCompany);
                    }
                }
            }
            return selectedCompany;
        }
    }
}
