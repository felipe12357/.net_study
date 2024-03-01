
using System.Text.Json.Serialization;

namespace MyApp.Models {

    public class Computer {
        //private string _motherboard; //esto es un campo
        //public string MotherBoard; //esto es una propiedad

        //una buena practica es q sean privadas
        //al hacer esto, c# automaticamente crea los metodos y el campo con "_" 
        //   private string MotherBoard { get => motherBoard; set => motherBoard = value}; 
        [JsonPropertyName("motherboard")]
        public string? MotherBoard {get; set;}


        [JsonPropertyName("cpu_cores")] //se utiliza esto para implementar el segundo metodo de mapeo
        public int? CPUCores {get;set;} 

        
        [JsonPropertyName("has_wifi")]
        public bool? HasWifi {get;set;} 

        
        [JsonPropertyName("release_date")]
        public DateTime? ReleaseDate {get;set;} 
        
        [JsonPropertyName("price")]
        public decimal? Price {get;set;}


        [JsonPropertyName("computer_id")]
        public int ComputerId {get;set;}
       
       
        [JsonPropertyName("has_lte")]
        public bool? HasLTE {get;set;}
       
       
        [JsonPropertyName("video_card")]
        //para evitar el approach del constructor tambien se puede hacer esto:
        public string? VideoCard {get;set;}  = "";

        //Esto se debe hacer para los tipo string y evitar el signo de "?"
        //Asi se genera el constructor
        public Computer(){
            if(VideoCard == null)
                VideoCard ="";

        }
    }
}