using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [ApiController] //permite enviar y recibir json
    [Route("[controller]")] //con esto hago q busque el nombre de la clase omitiendo la palabra controller
    //El nombre del controlador define el nombre de la ruta
    public class UserDapperController: ControllerBase{

        DataContextDapper _dapper;

        public UserDapperController(){
            _dapper = new DataContextDapper();
        }

        //el primer parametro sera el complemento a la ruta, en este caso GetUsers
        [HttpGet("GetUsers")] //aca la ruta es User/GetUsers
        public IEnumerable<UserComplete> GetUsers(){
            //Ejemplo utilizando los store procedures
            string query =  @"exec TutorialAppSchema.spGetUsers_withAverageSalary";
            return _dapper.LoadData<UserComplete>(query);
        }
       
    }

}