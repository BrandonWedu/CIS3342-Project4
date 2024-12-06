using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeListingAPI
{
    internal static class WriteUtility
    {
        internal static int CreateNew(int homeID, Utility utility)
        {
            DBConnect dbConnect = new DBConnect();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "P4_CreateNewUtility";

            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", (int)homeID, SqlDbType.Int, 8));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@utilityType", utility.Type.ToString(), SqlDbType.VarChar, 50));
            sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@utilityInformation", utility.Information, SqlDbType.VarChar));

            SqlParameter outputParam = DBParameterHelper.OutputParameter("@utilityID", SqlDbType.Int, 8);
            sqlCommand.Parameters.Add(outputParam);

            dbConnect.DoUpdate(sqlCommand);
            return (int)outputParam.Value;
        }

        internal static void DeleteUtility(int homeID)
        {
			try
			{
				DBConnect dbConnect = new DBConnect();
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "P4_DeleteUtility";
				sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@HomeID", (int)homeID, SqlDbType.Int, 8));
				dbConnect.DoUpdate(sqlCommand);
			}
			catch (SqlException sqlEx)
			{
				// Handle SQL specific exceptions, like issues with query, constraints, etc.
				Console.WriteLine($"SQL Exception: {sqlEx.Message}");
				throw; // Rethrow if you want it to bubble up or handle it differently
			}
			catch (Exception ex)
			{
				// Handle all other types of exceptions
				Console.WriteLine($"Exception: {ex.Message}");
				throw; // Rethrow the exception to be handled at a higher level or log it
			}
		}

        internal static void UpdateUtilities(int homeID, Utilities updatedUtilites, Utilities oldUtilites)
        {

            //Update or add utilities
            foreach (Utility currentUtility in updatedUtilites.List)
            {
                if (currentUtility.UtilityID != 0)
                {
                    DBConnect dbConnect = new DBConnect();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "P4_UpdateUtility";
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@UtilityID", (int)currentUtility.UtilityID, SqlDbType.Int, 8));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@HomeID", homeID, SqlDbType.Int, 8));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@UtilityType", currentUtility.Type.ToString(), SqlDbType.VarChar, 50));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@UtilityInformation", currentUtility.Information, SqlDbType.VarChar));
                    dbConnect.DoUpdate(sqlCommand);
                }
                else
                {
                    //Create new utility
                    DBConnect dbConnect = new DBConnect();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "P4_CreateNewUtility";
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@homeID", (int)homeID, SqlDbType.Int, 8));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@utilityType", currentUtility.Type.ToString(), SqlDbType.VarChar, 50));
                    sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<string>("@utilityInformation", currentUtility.Information, SqlDbType.VarChar));
                    SqlParameter outputParam = DBParameterHelper.OutputParameter("@utilityID", SqlDbType.Int, 8);
                    sqlCommand.Parameters.Add(outputParam);
                    dbConnect.DoUpdate(sqlCommand);
                }
            }


            //Remove deleted utilities
            foreach (Utility currentOldUtility in oldUtilites.List)
            {
                foreach (Utility currentUtility in updatedUtilites.List)
                {
                    // Every updated utility gets compared to each old utility
                    if (currentOldUtility.UtilityID != currentUtility.UtilityID)
                    {
                        //Delete the old utility because it wasnt present in the updated utilities 
                        DBConnect dbConnect = new DBConnect();
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "P4_RemoveUtility";
                        sqlCommand.Parameters.Add(DBParameterHelper.InputParameter<int>("@UtilityID", (int)currentOldUtility.UtilityID, SqlDbType.Int, 8));
                        dbConnect.DoUpdate(sqlCommand);
                    }
                }
            }
        }
    }
}
