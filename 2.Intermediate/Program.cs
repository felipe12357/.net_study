// See https://aka.ms/new-console-template for more information

using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyApp.Data;
using MyApp.Models;
using Newtonsoft.Json;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        static DataContextDapper dataDapper = new DataContextDapper(config);
        static DataContextEF dataEF = new DataContextEF(config);
        static void Main(string[] args)
        { 
            string sqlComand1 ="SELECT GETDATE()";
            DateTime rightnow =  dataDapper.loadSingle<DateTime>(sqlComand1);

            Console.WriteLine(rightnow);

            QueryDataWithEF();
           // QueryDataWithDapper();
            //InsertDataWithEF();
           
            //UpdateDataWithDapper();
            //DeleteDataWithDapper();
        }

        static void QueryDataWithEF(){
            IEnumerable<Computer> ?computersEF = dataEF.Computer?.ToList<Computer>();

            if(computersEF !=null) {
                int index =0;
                foreach(Computer computer in computersEF) {
                    Console.WriteLine(index + " " +JsonConvert.SerializeObject(computer));
                    index +=1;
                }
            }
        }

        static void InsertDataWithEF() {
            Computer myComputerInstance = new Computer(){
                MotherBoard = "Z890",
                CPUCores = 16,
                ReleaseDate = DateTime.Now,
                Price = 94.2m,
                VideoCard = "alpha 41"

            };

            dataEF.Add(myComputerInstance);
            dataEF.SaveChanges();
        }

        static void DeleteDataWithDapper() {
            string sqlInjection = "Delete from TutorialAppSchema.Computer where ComputerId = 5";
             dataDapper.ExecuteSql(sqlInjection);
        }

        static void UpdateDataWithDapper() {
            string sqlInjection = "UPDATE TutorialAppSchema.Computer SET CPUCores = 8 where ComputerId = 4";
             dataDapper.ExecuteSql(sqlInjection);
        }

        static void InserDataWithDapper() {
            Computer myComputerInstance = new Computer(){
                MotherBoard = "Z690",
                CPUCores = 16,
                ReleaseDate = DateTime.Now,
                Price = 94.2m,
                VideoCard = "alpha 40"

            };

            myComputerInstance.HasWifi = false;

            //@ es para permitir escribir en varias linea
            string sqlInjection = @"Insert into TutorialAppSchema.Computer ( Motherboard, HasWifi, HasLTE, ReleaseDate, Price, VideoCard)
            Values ('" + myComputerInstance.MotherBoard + "', '"
                    + myComputerInstance.HasWifi + "', '"
                    + false + "', '" 
                   // + myComputerInstance.ReleaseDate.ToString("yyyy-MM-dd") + "','"
                    + myComputerInstance.ReleaseDate.ToString("yyyy-MM-dd") + "','"
                    + myComputerInstance.Price + "','" 
                    + myComputerInstance.VideoCard + "' )";

            //Importante la conversion q se tuvo q hacer para insertar fechas
            Console.WriteLine(sqlInjection);
         //   dataDapper.ExecuteSql(sqlInjection);
        }

        static void QueryDataWithDapper(){
            string sqlComand ="SELECT * from TutorialAppSchema.Computer";
            List<Computer> dataComputers = dataDapper.LoadData<Computer>(sqlComand).ToList();

            int index =0;
            foreach(Computer computer in dataComputers) {
                Console.WriteLine(index + " " +JsonConvert.SerializeObject(computer));
                index +=1;

            }
          //  Console.WriteLine(JsonConvert.SerializeObject(dataComputers)); 
        }
    }
}
