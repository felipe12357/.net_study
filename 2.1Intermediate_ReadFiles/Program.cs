using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using MyApp.Data;
using MyApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyProject;
class Program
{
    static IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    static DataContextDapper dataDapper = new DataContextDapper(config);
    static DataContextEF dataEF = new DataContextEF(config);
    static void Main(string[] args)
    {
        string computersJson = readFile("Computers.json");
      //  mappearWithJsonSerializer(computersJson);
        mappearWithNewtonsoft(computersJson);
    }

    static void mappearWithNewtonsoft(string computersJson){
        IEnumerable<Computer> ? computersEnumerable = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);
        
        if(computersEnumerable != null) {
            //int index =0;
            // insertComputersDB(computersEnumerable);
            insertComputersDBEF(computersEnumerable);
            /* foreach(Computer computer in computersEnumerable) {
                Console.WriteLine(index + " " +JsonConvert.SerializeObject(computer));
                Console.WriteLine(index + " - " +computer.MotherBoard);
                index +=1;
            } */
        }

        JsonSerializerSettings settings = new JsonSerializerSettings() {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        writeFile(JsonConvert.SerializeObject(computersEnumerable),"computerswithNewton.txt");
        writeFile(JsonConvert.SerializeObject(computersEnumerable,settings),"computerswithNewtonSetting.txt");

    }

    static void mappearWithJsonSerializer(string computersJson){
        JsonSerializerOptions options = new JsonSerializerOptions(){
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }; 
       
        //OJO Q ESTO TIENE PROBLEMAS CON PROPIEDADES COMO CPUCore
        IEnumerable<Computer> ? computersEnumerable = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson, options);

        if(computersEnumerable != null) {
           // int index =0;
            insertComputersDB(computersEnumerable);
          //  insertComputersDBEF(computersEnumerable);
            /* foreach(Computer computer in computersEnumerable) {
                Console.WriteLine(index + " - " +computer.MotherBoard);
                string text = System.Text.Json.JsonSerializer.Serialize(computer);
                Console.WriteLine(index + " - " + text);
                index +=1;
            } */
        }

        writeFile(System.Text.Json.JsonSerializer.Serialize(computersEnumerable),"computerswithSystemText.txt");
        writeFile(System.Text.Json.JsonSerializer.Serialize(computersEnumerable,options),"computerswithSystemTextSettings.txt");
    }

    static void insertComputersDB( IEnumerable<Computer> computersEnumerable){
        string sqlInjection =  @"Insert into TutorialAppSchema.Computer ( Motherboard,HasWifi,HasLTE,Price,ReleaseDate,VideoCard) Values";
        foreach(Computer computer in computersEnumerable) {

            sqlInjection += @"('"  + computer.MotherBoard + "' , '"
                        + computer.HasWifi + "', '"
                        + computer.HasLTE + "', '"
                        + convertNumber(computer.Price.ToString()) + "', '"
                        + computer.ReleaseDate?.ToString("yyyy-MM-dd") + "', '"
                        + computer.VideoCard?.Replace("'", "''") + "' ),";
        }

        Console.WriteLine(sqlInjection);
        //lo uso para eliminar la ultima "," ya q no es valida en sql
        sqlInjection = sqlInjection.Remove(sqlInjection.Length - 1);
        dataDapper.ExecuteSql(sqlInjection); 
    }

    static void insertComputersDBEF(IEnumerable<Computer> computersEnumerable){
        foreach(Computer computer in computersEnumerable) {
            computer.ComputerId = 0;
            dataEF.Add(computer);
        }
        dataEF.SaveChanges();
    }

    static int? convertNumber(string? val){
        if(val== "" || val ==null){
            return null;
        }else{
             return int.Parse(val);
        }
    }


    static string readFile(string fileName){
        string dataString = File.ReadAllText(fileName);
        // Console.WriteLine(dataString);
        return dataString;
    }

    static void writeFile(string text,string fileName) {
        // De esta forma crea una archivo nuevo SIEMPRE y pone el contenido
        File.WriteAllTextAsync(fileName,text);
    
        //De esta form agrega contenido a un archivo.. si no existe lo crea
        //using StreamWriter openFile = new(fileName,append:true);
        //openFile.WriteLine("\n text");

        //openFile.Close();
    }
}
