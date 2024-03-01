
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models {

    public class Computer {
        //private string _motherboard; //esto es un campo
        //public string MotherBoard; //esto es una propiedad

        //una buena practica es q sean privadas
        //al hacer esto, c# automaticamente crea los metodos y el campo con "_" 
        //   private string MotherBoard { get => motherBoard; set => motherBoard = value}; 
        public string? MotherBoard {get; set;} 
        public int? CPUCores {get;set;} 
        public bool? HasWifi {get;set;} 
        public DateTime? ReleaseDate {get;set;} 
        public decimal? Price {get;set;}
        public int ComputerId {get;set;}
        public bool? HasLTE {get;set;}
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