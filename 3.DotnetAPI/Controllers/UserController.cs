using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
namespace DotnetAPI.Controllers;


[ApiController] //permite enviar y recibir json
[Route("[controller]")] //con esto hago q busque el nombre de la clase omitiendo la palabra controller
//El nombre del controlador define el nombre de la ruta
public class UserController: ControllerBase{

    DataContextDapper _dapper;

    public UserController(){
        _dapper = new DataContextDapper();
    }
    
    [HttpGet("{userId}")] //el parametro entre {} se utiliza para la ruta sea : User/34
    //[HttpGet("userId")] //de lo contrario la ruta es User/UserId?userId=32
    //importante el parametro "{userId}" debe coincidir con int userId
    public User GetUser(int userId){
        string query =  @"
                SELECT * 
                FROM DotNetCourseDatabase.TutorialAppSchema.Users
                Where UserId = '"+userId+"' ";
        return _dapper.loadSingle<User>(query);
    }

    //el primer parametro sera el complemento a la ruta, en este caso GetUsers
    [HttpGet("GetUsers")] //aca la ruta es User/GetUsers
    public IEnumerable<User> GetUsers(){
        string query =  @"
                SELECT * 
                FROM DotNetCourseDatabase.TutorialAppSchema.Users";
        return _dapper.LoadData<User>(query);
    }

    [HttpPut("{userId}")]
    public IActionResult UpdateUser(int userId,UserDto user){
        string query =  @"
                UPDATE TutorialAppSchema.Users 
                SET FirstName = '" + user.FirstName + "'"
                + ", LastName = '" + user.LastName + "'"
                + ", Email = '" + user.Email + "'"
                + ", Gender = '" + user.Gender + "'"
                + ", Active = '" + user.Active + "'"
                + " Where UserId = '"+userId+"' ";
        
        int rowsAffected = _dapper.ExecuteSql(query);

        if(rowsAffected>0)
            return Ok();
        
        throw new Exception("Failed to update User");
    }

    [HttpPost("")]
    public IActionResult InsertUser(UserDto user){
        string query =  @"
          insert into TutorialAppSchema.Users(
                FirstName,LastName,Email,Gender)
            Values( '" + user.FirstName + "' , '" + user.LastName + "', '" + user.Email + "', '" + user.Gender + "' )";
        
        int rowsAffected = _dapper.ExecuteSql(query);
        Console.WriteLine(query);
        if(rowsAffected==1)
            return Ok();
        
        throw new Exception("Failed to insert User");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId){

        string query =  @"Delete from TutorialAppSchema.Users where UserId = " + userId;
        Console.WriteLine(query);
        int rowsAffected = _dapper.ExecuteSql(query);

         if(rowsAffected>0)
            return Ok();
        
        throw new Exception("Failed to delete User");

    }

}