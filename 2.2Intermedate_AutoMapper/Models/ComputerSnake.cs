
namespace MyApp.Models {

    public class ComputersSnake {
        //private string _motherboard; //esto es un campo
        //public string MotherBoard; //esto es una propiedad

        //una buena practica es q sean privadas
        //al hacer esto, c# automaticamente crea los metodos y el campo con "_" 
        //   private string MotherBoard { get => motherBoard; set => motherBoard = value}; 
        public string? motherboard {get; set;} 
        public int? cpu_cores {get;set;} 
        public bool? has_wifi {get;set;} 
        public DateTime? release_date {get;set;} 
        public decimal? price {get;set;}
        public int computer_id {get;set;}
        public bool? has_lte {get;set;}
        //para evitar el approach del constructor tambien se puede hacer esto:
        public string? video_card {get;set;}  = "";

        //Esto se debe hacer para los tipo string y evitar el signo de "?"
        //Asi se genera el constructor
        public ComputersSnake(){
            if(video_card == null)
                video_card ="";

        }
    }
}