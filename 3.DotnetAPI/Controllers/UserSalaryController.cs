using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
namespace DotnetAPI.Controllers;

[ApiController] //permite enviar y recibir json
[Route("[controller]")] //con esto hago q busque el nombre de la clase omitiendo la palabra controller
//El nombre del controlador define el nombre de la ruta
public class UserSalaryController: ControllerBase{

    DataContextDapper _dapper;

    public UserSalaryController(){
        _dapper = new DataContextDapper();
    }

    [HttpGet("{userId}")] //el parametro entre {} se utiliza para la ruta sea : UserSalary/34
    //[HttpGet("userId")] //de lo contrario la ruta es UserSalary/UserId?userId=32
    //importante el parametro "{userId}" debe coincidir con int userId
    public UserSalary GetUserSalary(int userId){
        string query =  @"
                SELECT * 
                FROM TutorialAppSchema.UserSalary
                Where UserId = '"+userId+"' ";
        return _dapper.loadSingle<UserSalary>(query);
    }

    //el primer parametro sera el complemento a la ruta, en este caso GetUsers
    [HttpGet("GetUsersSalary")] //aca la ruta es UserSalary/GetUsersSalary
    public IEnumerable<UserSalary> GetUsersSalary(){
        string query =  @"
                SELECT * 
                FROM TutorialAppSchema.UserSalary";
        return _dapper.LoadData<UserSalary>(query);
    }

    [HttpPut("{userId}")]
    public IActionResult UpdateUserSalary(int userId,UserSalary userJobInfo){
        string query =  @"
                UPDATE TutorialAppSchema.UserSalary 
                SET Salary = '" + userJobInfo.Salary + "'"
                + " Where UserId = '"+userId+"' ";
        
        int rowsAffected = _dapper.ExecuteSql(query);

        if(rowsAffected>0)
            return Ok();
        
        throw new Exception("Failed to update UserSalary");
    }

    [HttpPost("")]
    public IActionResult InsertUserSalary(UserSalary userSalary){
        string query =  @"
          insert into TutorialAppSchema.UserSalary(
                Salary,UserId)
            Values( '" + userSalary.Salary + "' , '" + userSalary.UserId + "' )";
        
        int rowsAffected = _dapper.ExecuteSql(query);
        Console.WriteLine(query);
        if(rowsAffected==1)
            return Ok();
        
        throw new Exception("Failed to insert UserSalary");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUserSalary(int userId){

        string query =  @"Delete from TutorialAppSchema.UserSalary where UserId = " + userId;
        Console.WriteLine(query);
        int rowsAffected = _dapper.ExecuteSql(query);

         if(rowsAffected>0)
            return Ok();
        
        throw new Exception("Failed to delete UserSalary");

    }
}