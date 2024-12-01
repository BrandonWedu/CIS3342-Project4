﻿using System.Data;
using Microsoft.Data.SqlClient;

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

        internal static void UpdateUtilities(int homeID, Utilities updatedUtilites)
        {
            foreach (Utility currentUtility in updatedUtilites.List)
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
        }
    }
}
