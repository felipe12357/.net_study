using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Data
{
    public class DataContextDapper
    {
        private string?  _connectionString;
        public DataContextDapper()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString =  config.GetConnectionString("DefaultConnection");
        }

         public IEnumerable<T> LoadData<T>(string sqlText){
          
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            IEnumerable<T> data = dbConnection.Query<T>(sqlText);
            dbConnection.Close();
            return data;
        }

        public T loadSingle<T>(string sqlText){
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            T data = dbConnection.QuerySingle<T>(sqlText);
            dbConnection.Close();
            return data;
        }

        public IEnumerable<T> LoadDataWithParameters<T>(string sql, DynamicParameters parameters){
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Query<T>(sql, parameters);
        }
        public T LoadDataSingleWithParameters<T>(string sql, DynamicParameters parameters){
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.QuerySingle<T>(sql, parameters);
        } 
        public int ExecuteSql(string sqlInjection) {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            int rowsAffected = dbConnection.Execute(sqlInjection);
            dbConnection.Close();
            return rowsAffected;
        }
    }
}
