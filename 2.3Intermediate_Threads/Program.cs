class Program {
    static async Task Main(string[] args){
        // await example(); //con esto espero a q terminé la tarea 1 para continuar

        Task secondTask = ConsoleAfterDelayAsync("Task 2", 150);
        Task thirdTask = ConsoleAfterDelayAsync("Task 3", 50);
        await example(); //lo pongo aca y lanzo los 3 timers al tiempo.. cada uno termina segun q tanto delay tengan asignados
        await secondTask; 
        await thirdTask;
        Console.WriteLine("Terminó");
    }

    static async Task ConsoleAfterDelayAsync(string text, int delayTime){
        //Thread.Sleep(delayTime);
        await Task.Delay(delayTime);
        Console.WriteLine(text);
    }


    //Primer Ejemplo
    static async Task example(){
        Task firstTask = new Task(()=>{
            Thread.Sleep(100);
            Console.WriteLine("Task 1");
        });
        Console.WriteLine("muestra inmediato la tarea no ha iniciado" );
        firstTask.Start();
         Console.WriteLine("aca no espera");
         await firstTask; //espera a q se acabe el hilo y sigue con el proceso
        Console.WriteLine("Termina Tarea 1");
    }


}