
using AutoMapper;
using MyApp.Models;
namespace MyApp;

class Program {
    static void Main(string[] args){

        //Programa q hace un SEED a la bd
        Seed seed = new Seed(){};
        seed.Run();

        //Ejemplo básico de mappeo
        // mappingExample();
    }

    static void mappingExample(){
        string computersSnakeJson = readFile("ComputersSnake.json");
        IEnumerable<ComputersSnake>? computersDeserialized =  System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ComputersSnake>>(computersSnakeJson);
        IEnumerable<Computer>? computersMappedbyJsonPropertyName = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersSnakeJson);

        if(computersDeserialized != null){
            IEnumerable<Computer> transformedModel = mapComputerSnakeModelIntoComputerModel(computersDeserialized);
            foreach(Computer computer in transformedModel){
                Console.WriteLine(computer.MotherBoard + '-' + computer.CPUCores);
            }
        }
        Console.WriteLine("--------------------------");
        //Gracias a q usamos la propiedad JsonPropertyName en el modelo es posible hacer el mappeo directamente
        if(computersMappedbyJsonPropertyName !=null){
            foreach(Computer computer in computersMappedbyJsonPropertyName){
                Console.WriteLine(computer.MotherBoard + '-' + computer.CPUCores);
            }
        }
    }

    static IEnumerable<Computer> mapComputerSnakeModelIntoComputerModel(IEnumerable<ComputersSnake> computers){

        Mapper mapper = new Mapper(new MapperConfiguration((cfg)=>{
            cfg.CreateMap<ComputersSnake,Computer>()
            .ForMember(destination => destination.ComputerId, options => options.MapFrom(source => source.computer_id) )
            .ForMember(destination => destination.MotherBoard, options => options.MapFrom(source => source.motherboard))
            .ForMember(destination => destination.CPUCores, options => options.MapFrom(source => source.cpu_cores))
            .ForMember(destination => destination.HasLTE, options => options.MapFrom(source => source.has_lte))
            .ForMember(destination => destination.HasWifi, options => options.MapFrom(source => source.has_wifi))
            .ForMember(destination => destination.VideoCard, options => options.MapFrom(source => source.video_card))
            .ForMember(destination => destination.Price, options => options.MapFrom(source => source.price))
            .ForMember(destination => destination.ReleaseDate, options => options.MapFrom(source => source.release_date));
        }));

        IEnumerable<Computer> computerResult;
        return computerResult = mapper.Map<IEnumerable<Computer>>(computers);
    }

    static string readFile(string fileName){
        string dataString = File.ReadAllText(fileName);
        return dataString;
    }
}