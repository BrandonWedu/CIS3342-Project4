using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public static class DBParameterHelper
    {
        public static SqlParameter InputParameter<T>(string parameter, T value, SqlDbType type, int size)
        {
            SqlParameter inputParameter = new SqlParameter(parameter, type, size);
            inputParameter.Value = value;
            inputParameter.Direction = ParameterDirection.Input;
            return inputParameter;
        }
        public static SqlParameter InputParameter<T>(string parameter, T value, SqlDbType type)
        {
            SqlParameter inputParameter = new SqlParameter(parameter, type);
            inputParameter.Value = value;
            inputParameter.Direction = ParameterDirection.Input;
            return inputParameter;
        }
        public static SqlParameter OutputParameter(string parameter, SqlDbType type, int size)
        {
            SqlParameter outputParameter = new SqlParameter(parameter, type, size);
            outputParameter.Direction = ParameterDirection.Output;
            return outputParameter;
        }
    }
}
