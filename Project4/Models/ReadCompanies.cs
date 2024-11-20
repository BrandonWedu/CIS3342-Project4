using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
	public class ReadCompanies
	{
		private DBConnect databaseHandler = new DBConnect();
		private CompanyList allCompanies = new CompanyList();

		public ReadCompanies()
		{
			allCompanies = new CompanyList();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "SelectAllCompanies";
			DataTable agentContactData = databaseHandler.GetDataSet(sqlCommand).Tables[0];

			foreach (DataRow row in agentContactData.Rows)
			{
				allCompanies.Add(new Company((int)row["CompanyID"], row["CompanyName"].ToString(), Serializer.DeserializeData<Address>((byte[])row["CompanyAddress"]), row["PhoneNumber"].ToString(), row["Email"].ToString()));
			}
		}

		public CompanyList GetAllCompanies()
		{
			return allCompanies;
		}

		public CompanyList GetCompanyByCompanyID(int id)
		{
			CompanyList selectedCompany = new CompanyList();
			foreach (Company currentCompany in allCompanies.List)
			{
				if (currentCompany.CompanyID == id)
				{
					selectedCompany.Add(currentCompany);
				}
			}
			return selectedCompany;
		} 
	}
}
