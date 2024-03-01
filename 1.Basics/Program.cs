// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;


//Explicacion clases staticas
//https://es.stackoverflow.com/questions/152642/static-que-es-y-para-que-sirve

namespace HelloWorld
{
    public class Program{

        //como el methodo principal es estatico hace q todo deba ser estatico para poder ser accesible
        static int Num = 301; //Las variables q estan a nivel de clase deben iniciar con mayuscula
        //buena forma de diferenciar las variables de clase y las de los metodos
        public static void Main(string[] args){
             
            List<int> myNumberList = new List<int>(){
                2, 3, 5, 6, 7, 9, 10, 123, 324, 54
            };

            int test = Num+31;
            //como el methodo principal es estatico hace q todo deba ser estatico para poder ser accesible
            printPareValues(myNumberList);   
        }

        private static void printPareValues(List<int> myNumberList ){
           
            
            foreach(int num in myNumberList){
                if(num%2 ==0)
                    Console.WriteLine(num);
            }
        }

        private static void loopsExamples(){
            //Corre los pares
            List<int> myNumberList = new List<int>(){
                2, 3, 5, 6, 7, 9, 10, 123, 324, 54
            };
            
            foreach(int num in myNumberList){
                if(num%2 ==0)
                    Console.WriteLine(num);
            }


            //Write Your Code Here
            DateTime startTime = DateTime.Now;
           
            int[] myIntArray = new int[] {10,20,21,31,7};
            int resultFor=0;
            int resultForeach=0;
            int resultWhile =0;


            for(int a =0; a<myIntArray.Length;a++){
                resultFor+=myIntArray[a];
            }

            foreach(int value in myIntArray) {
                resultForeach+=value;
            }

            int index=0;
            while(index<myIntArray.Length){
                resultWhile +=myIntArray[index];
                index++;
            }

            int index2=0;
            int resultDoWhile =0;
            do{
               resultDoWhile +=myIntArray[index2];
                index2++; 
            }while(index2<myIntArray.Length);
            

            Console.WriteLine(resultFor);
            Console.WriteLine(startTime);
            Console.WriteLine(resultForeach);
            Console.WriteLine(resultWhile);
             Console.WriteLine(resultDoWhile);
        }

        private static void switchCaseExample(){

            int myInt1 = 5;
            myInt1 -=6;

            string myString = "Andres";
            myString += " Felipe"; 

            Boolean isEqual = (myInt1 == 1);

            Console.WriteLine(myInt1.Equals(-1));
            Console.WriteLine(myInt1);
            Console.WriteLine(myString);
            Console.WriteLine(isEqual);

            string girl ="Natalia";

            //en los cases tiene q ser una constanste no puede usarse una variable
            switch(girl){
                case "lina":
                    Console.WriteLine("your girl is LIna");
                    break;
                case "Natalia":
                    Console.WriteLine("your girl is natalia");
                    break;
                default:
                    Console.WriteLine("girl not found");
                    break;
            }
        }

        private static void variables()  {
            decimal myDecimal = 12.3m;
            string myString ="HOLA Mundo hpta";
            bool myBoolean = true;
            float myFloat = 12.1f;
            double myDouble = 12.1;
            int myInt= 22;

            //los arrays tienen una longitud definida y no se puede cambiar
            string[] myArray = new string[2];
            myArray[1] = "HOLA MUNDO";
            //tambien se puede definir asi:
            string[] myArray2 = {"apples","oranges","watermelon","grapes","raspberry"};

            //las listas son totalmente dinamicas por lo tanto se pueden añadir o eliminar elementos y modificar su tamaño
            List<string> myfruitsList = new List<string>() {"coco","banana"} ;
            myfruitsList.Add("Watermelon");

            //solo se puede asignar a una lista o un array ya definido
            IEnumerable<string> myEnumerable = new List<string>() {"Erik","Pilar","Natalia"};

            //el primer valor es el key por el cual podemos buscar
            Dictionary<int,string> myDictionary = new Dictionary<int, string>() {
                {0,"Erik"},
                {1,"Pilar"}
            };

            Dictionary<int,string[]> myDictionary2 = new Dictionary<int, string[]>() {
                {0, new string[]{"Erik", "Pilar"}},
                {1, new string[]{"Natalia", "Valentina"}}
            };
              
            
            
            //Write You Code Above This Line
            Console.WriteLine(myString);
            Console.WriteLine(myDecimal);
            Console.WriteLine(myBoolean);
            Console.WriteLine(myFloat);
            Console.WriteLine(myDouble);
            Console.WriteLine(myInt);
            Console.WriteLine(myArray);
             Console.WriteLine(myArray[1]);
            Console.WriteLine(myArray2[0]);
            Console.WriteLine(myfruitsList);
            Console.WriteLine(myfruitsList[0]);
            Console.WriteLine(myfruitsList[2]);
            Console.WriteLine(myDictionary[0]);
             Console.WriteLine(myDictionary2[1][0]);
        }
    }
}
