using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyApp.Models;

namespace MyApp.Data {
    public class DataContextDapper{

        private string ? _connectionStringMac;

        public DataContextDapper(IConfiguration config) {
            _connectionStringMac =  config.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<T> LoadData<T>(string sqlText){
          
            IDbConnection dbConnection = new SqlConnection(_connectionStringMac);
            IEnumerable<T> data = dbConnection.Query<T>(sqlText);
            dbConnection.Close();
            return data;
        }

        public T loadSingle<T>(string sqlText){
            IDbConnection dbConnection = new SqlConnection(_connectionStringMac);
            T data = dbConnection.QuerySingle<T>(sqlText);
            dbConnection.Close();
            return data;
        }

        public int ExecuteSql(string sqlInjection) {
            IDbConnection dbConnection = new SqlConnection(_connectionStringMac);
            int rowsAffected = dbConnection.Execute(sqlInjection);
            Console.WriteLine("inserted");
            dbConnection.Close();
            return rowsAffected;
        }
    }
}