using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyApp.Models;

namespace MyApp.Data {
    public class DataContextDapper{

        //para windows solo es necesario  Trusted_Connection=true; no es necesario poner ids y passwords
        //string connectionStringWindows = "Server = localhost; Database = DotNetCourseDatabase; TrustServerCertificate = true; Trusted_Connection=true;";
        private string ? _connectionStringMac = "Server = localhost; Database = DotNetCourseDatabase; TrustServerCertificate = true; Trusted_Connection=false; User Id = SA ; Password=SQLConnect1";

        public DataContextDapper(IConfiguration config) {
            //redefino la variable de confirguracion utilizando el approach de tener la key en appsettings
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