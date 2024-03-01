using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;


[ApiController] //permite enviar y recibir json
[Route("[controller]")] //con esto hago q busque el nombre de la clase omitiendo la palabra controller
//El nombre del controlador define el nombre de la ruta
public class WeatherForecastController: ControllerBase{

    private readonly string[] _summaries = new[] { 
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    //el primer parametro sera el complemento a la ruta, en este caso ninguno
    [HttpGet("",Name = "GetWeatherForecast")] //la ruta es /WeatherForecast
    public IEnumerable<WeatherForecast> GetFiveDayForecast(){
        IEnumerable<WeatherForecast> forecast =  Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                _summaries[Random.Shared.Next(_summaries.Length)]
            ))
            .ToArray();
        return forecast;
    }
}


public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
